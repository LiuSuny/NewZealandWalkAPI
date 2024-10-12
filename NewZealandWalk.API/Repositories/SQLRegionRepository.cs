using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NewZealandWalksDbContext dbContext;
        public SQLRegionRepository(NewZealandWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
           string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var regions = dbContext.Regions.AsQueryable();
            // Filtering


            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Code", StringComparison.OrdinalIgnoreCase))
                {
                    regions = regions.Where(x => x.Code.Contains(filterQuery));
                }
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regions = regions.Where(x => x.Name.Contains(filterQuery));
                }

            }
            // Sorting 
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {

                if (sortBy.Equals("Code", StringComparison.OrdinalIgnoreCase))
                {
                    regions = isAscending ? regions.OrderBy(x => x.Name) : regions.OrderByDescending(x => x.Code);
                }
                else if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regions = isAscending ? regions.OrderBy(x => x.Name) : regions.OrderByDescending(x => x.Name);
                }
            }
            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await regions.Skip(skipResults).Take(pageSize).ToListAsync();

            //return await dbContext.Regions.ToListAsync();
        }
        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
       
        
    }
}
