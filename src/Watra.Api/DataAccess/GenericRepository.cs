// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BBT.MaybePattern;
    using Microsoft.EntityFrameworkCore;
    using Watra.Api.DataAccess.DbContext;
    using Watra.Api.Validation;

    /// <summary>
    /// Implementation of <see cref="IGenericRepository{TDbModel, TDto}"/>.
    /// Conversion between <typeparamref name="TDbModel"/> and <typeparamref name="TDto"/> are
    /// done using automapper, see <see cref="Mappings.MasterDataMapProfile"/>.
    /// The following operations are saved immediately:
    ///  - Create
    ///  - Update
    ///  - Delete
    /// .
    /// </summary>
    /// <typeparam name="TDbModel"><typeparamref name="TDbModel"/> on <see cref="IGenericRepository{TDbModel, TDto}"/>.</typeparam>
    /// <typeparam name="TDto"><typeparamref name="TDto"/> on <see cref="IGenericRepository{TDbModel, TDto}"/>.</typeparam>
    public class GenericRepository<TDbModel, TDto> : IGenericRepository<TDbModel, TDto>
        where TDbModel : class, IDbEntity, new()
        where TDto : class, new()
    {
        private readonly WatraContext context;
        private readonly DbSet<TDbModel> table;
        private readonly IMapper mapper;
        private readonly IValidator<TDto> dtoValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TDbModel, TDto}"/> class.
        /// </summary>
        public GenericRepository(WatraContext context, IMapper mapper, IValidator<TDto> dtoValidator)
        {
            this.context = context;
            this.table = context.Set<TDbModel>();
            this.mapper = mapper;
            this.dtoValidator = dtoValidator;
        }

        /// <inheritdoc/>
        public ICollection<TDto> GetAll(params Expression<Func<TDto, object>>[] membersToExpand)
        {
            return this.table.ProjectTo<TDto>(this.mapper.ConfigurationProvider, membersToExpand).ToList();
        }

        /// <inheritdoc/>
        public TDto GetById(int id, params Expression<Func<TDto, object>>[] membersToExpand)
        {
            var dto = this.ProjectGetById(id, membersToExpand);

            return dto;
        }

        /// <inheritdoc/>
        public TDto Insert(TDto obj, params Expression<Func<TDto, object>>[] membersToExpand)
        {
            this.dtoValidator.AssertIsValid(obj);

            var dbModel = this.mapper.Map<TDto, TDbModel>(obj);

            // Because of the AutoMapper mapping we are forced to
            // reset the ID to prevent an identity insert exception from EF Core.
            dbModel.Id = 0;
            this.context.Attach(dbModel);
            this.context.SaveChanges();

            return this.ProjectGetById(dbModel.Id, membersToExpand);
        }

        /// <inheritdoc/>
        public TDto Update(TDto obj, int id, params Expression<Func<TDto, object>>[] membersToExpand)
        {
            this.dtoValidator.AssertIsValid(obj);

            var dbModel = this.table.Find(id);
            this.mapper.Map<TDto, TDbModel>(obj, dbModel);

            this.context.Entry(dbModel).State = EntityState.Modified;

            this.context.SaveChanges();

            return this.ProjectGetById(dbModel.Id, membersToExpand);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            TDbModel existing = this.table.Find(id);
            this.table.Remove(existing);
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public void MarkDeleted(TDto obj)
        {
            this.context.Remove(obj);
        }

        private TDto ProjectGetById(int id, params Expression<Func<TDto, object>>[] membersToExpand)
        {
            var dto = this.table.Where(model => model.Id == id).ProjectTo(this.mapper.ConfigurationProvider, membersToExpand).SingleOrDefault();

            return Maybe.Some(dto).ValueOrException(nameof(id), $"{nameof(TDbModel)} with ID {id} not found");
        }
    }
}
