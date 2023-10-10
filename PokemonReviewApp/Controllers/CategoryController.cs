using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await categoryRepository.GetCategories();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<CategoryDto>>(categories));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (!await categoryRepository.CategoryExists(id))
            {
                return NotFound();
            }

            var category = await categoryRepository.GetCategory(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<CategoryDto>(category));
        }

        [HttpGet]
        [Route("{id}/pokemonList")]
        public async Task<IActionResult> GetPokemonByCategory([FromRoute] int id)
        {
            if (!await categoryRepository.CategoryExists(id))
            {
                return NotFound();
            }

            var pokemonList = await categoryRepository.GetPokemonByCategory(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(mapper.Map<List<PokemonDto>>(pokemonList));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);
            }
            var existingCategory = await categoryRepository.HasCategory(category.Name);
            if (existingCategory != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = mapper.Map<Category>(category);
            if (!await categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }
    }
}
