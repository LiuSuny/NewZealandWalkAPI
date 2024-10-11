using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.Domain;
using NewZealandWalk.API.Models.Dto;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsControllerController : ControllerBase
    {

       // private readonly NewZealandWalksDbContext dbContext;
        private readonly IRegionRepository repository;
        private readonly IMapper mapper;

        public RegionsControllerController(IRegionRepository repository,  IMapper mapper)
        {
           
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            // Get Data From Database - Domain models
            var regionsDomain = await repository.GetAllAsync();

            // Map Domain Models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}
            // Return DTOs
            return Ok(regionsDto);

        }

        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<ActionResult> GetById([FromRoute] Guid id)
        {
            // var region = dbContext.Regions.Find(id);
            // Get Region Domain Model From Database
            var regionDomain = await repository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            // Map/Convert Region Domain Model to Region DTO
            // 
            var regionDto = mapper.Map<RegionDto>(regionDomain);
           
            // Return DTO back to client
            return Ok(regionDto);
        }

        // POST To Create New Region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to create Region
            regionDomainModel = await repository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            // Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await repository.UpdateAsync(id, regionDomainModel);

            // Check if region exists
            regionDomainModel = await repository.UpdateAsync(id, regionDomainModel);
           
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        // Delete Region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await repository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // map Domain Model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
