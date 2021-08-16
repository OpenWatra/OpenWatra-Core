// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Api.DataAccess.DbContext
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Main database context for WaTra API backend.
    /// </summary>
    public class WatraContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatraContext"/> class.
        /// </summary>
        public WatraContext(DbContextOptions<WatraContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="DbHoseConnector"/>.
        /// </summary>
        public DbSet<DbHoseConnector> HoseConnectors { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="DbHose"/>.
        /// </summary>
        public DbSet<DbHose> Hoses { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="DbPump"/>.
        /// </summary>
        public DbSet<DbPump> Pumps { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="DbWatraRoute"/>.
        /// </summary>
        public DbSet<DbWatraRoute> WatraRoutes { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> for <see cref="DbPumpSelections"/>.
        /// </summary>
        public DbSet<DbPumpSelection> PumpSelections { get; set; }

        /// <summary>
        /// Override with model configurations, see <see cref="DbContext.OnModelCreating(ModelBuilder)"/>.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPump>()
                .HasMany(pump => pump.PumpPressureFlowModel)
                .WithOne(pressureFlowModel => pressureFlowModel.Pump)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DbWatraRoute>()
                .HasMany(watraRoute => watraRoute.PumpSelections)
                .WithOne(pumpSelection => pumpSelection.WatraRoute)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DbWatraRoute>()
                .HasMany(watraRoute => watraRoute.WatraRouteDistanceHeightElements)
                .WithOne(watraRouteDistanceHeightElements => watraRouteDistanceHeightElements.WatraRoute)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
