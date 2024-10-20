﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.CustomActionFilters;
using NewZealandWalk.API.Models.Domain;
using NewZealandWalk.API.Models.Dto;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        private readonly ILogger<RegionsController> logger;
        public WalksController(IMapper mapper, IWalkRepository walkRepository, ILogger<RegionsController> logger)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
            this.logger = logger;
        }
        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
       // [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await walkRepository.CreateAsync(walkDomainModel);
                // Map Domain model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            return BadRequest();
            
        }
        // GET Walks
        // GET: /api/walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //var walksDomainModel = await walkRepository.GetAllAsync();
            

            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,
                   isAscending ?? true, pageNumber, pageSize);

            // Create an exception
            //throw new Exception("This is a new exception");

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        // Get Walk By Id
        // GET: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
        // Update Walk By Id
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
       // [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                // Map Domain Model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            return BadRequest();

        }
        // Delete a Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
