// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Watra.Api.DataAccess.DbContext;

    /// <summary>
    /// Generic repository with automatic mapping between DTO
    /// and DB classes. To extend mappings, adapt <see cref="Mappings.MasterDataMapProfile"/>.
    /// </summary>
    /// <typeparam name="TDbModel">Database object.</typeparam>
    /// <typeparam name="TDto">Data transfer object.</typeparam>
    public interface IGenericRepository<TDbModel, TDto>
        where TDbModel : class, IDbEntity, new()
        where TDto : class, new()
    {
        /// <summary>
        /// Gets all of the <typeparamref name="TDto"/> from the database.
        /// </summary>
        ICollection<TDto> GetAll(params Expression<Func<TDto, object>>[] membersToExpand);

        /// <summary>
        /// Gets the <typeparamref name="TDto"/> from the database which
        /// has the ID <paramref name="id"/>.
        /// </summary>
        TDto GetById(int id, params Expression<Func<TDto, object>>[] membersToExpand);

        /// <summary>
        /// Inserts the <typeparamref name="TDto"/> into the database.
        /// </summary>
        TDto Insert(TDto obj, params Expression<Func<TDto, object>>[] membersToExpand);

        /// <summary>
        /// Updates the <see cref="TDto"/> in the database.
        /// The record is identified by the <paramref name="id"/>.
        /// </summary>
        TDto Update(TDto obj, int id, params Expression<Func<TDto, object>>[] membersToExpand);

        /// <summary>
        /// Marks the DB entry behind <paramref name="obj"/> as deleted.
        /// </summary>
        void MarkDeleted(TDto obj);

        /// <summary>
        /// Deletes the <typeparamref name="TDto"/> with the ID
        /// <typeparamref name="TDto"/> in the database.
        /// </summary>
        void Delete(int id);
    }
}
