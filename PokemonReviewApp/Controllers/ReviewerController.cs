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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository reviewerRepository;
        private readonly IMapper mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            this.reviewerRepository = reviewerRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetReviewers()
        {
            var reviewers = await reviewerRepository.GetReviewers();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<ReviewerDto>>(reviewers));
        }

        [HttpGet]
        [Route("{reviewerId:int}")]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            if (!await reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var reviewer = await reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<ReviewerDto>(reviewer));
        }

        [HttpGet]
        [Route("{reviewerId:int}/reviews")]
        public async Task<IActionResult> GetReviewsByReviewer(int reviewerId)
        {
            if (!await reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var reviews = await reviewerRepository.GetReviewsByReviewer(reviewerId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(mapper.Map<List<ReviewDto>>(reviews));
        }
    }
}
