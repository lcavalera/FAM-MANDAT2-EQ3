using Acef.Reasons.ApplicationCore.Entities.DTO;
using Acef.Reasons.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Acef.Reasons.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReasonsController : ControllerBase
    {
        private readonly IReasonService _reasonService;

        public ReasonsController(IReasonService reasonService)
        {
            _reasonService = reasonService;
        }

        /// <summary>
        /// Returns a list of consultation reasons
        /// </summary>
        /// <response code="200">list of consultation reasons found and returned</response>
        /// <response code="404">list of consultation reasons not found</response>
        /// <response code="500">service currently unavailable</response>
        // GET: api/<ReasonController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReasonDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<ReasonDTO>> Get()
        {
            return await _reasonService.Get();
        }

        /// <summary>
        /// Returns a specific consultation reason from its id
        /// </summary>
        /// <param name="id">id of the consultation reason to return</param>
        /// <response code="200">consultation reason found and returned</response>
        /// <response code="400">Bad Request : Element has been soft deleted</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // GET api/<ReasonController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReasonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReasonDTO>> Get(int id)
        {
            ReasonDTO reason = await _reasonService.GetById(id);

            return reason == null ?
                NotFound(new { Erreur = $"Consultation reason not found (id = {id})" }) : Ok(reason);
        }

        /// <summary>
        /// Adds a consultation reason to the database
        /// </summary>
        /// <param name="reason">the consultation reason to add</param>
        /// <response code="201">consultation reason added successfully</response>
        /// <response code="400">invalid Model, bad request</response>
        /// <response code="500">service currently unavailable</response>
        // POST api/<ReasonController>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ReasonDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ReasonDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] ReasonDTO reason)
        {
            if (ModelState.IsValid)
            {
                await _reasonService.Add(reason);

                //return CreatedAtAction("Get", new { id = reason.ID }, reason);
                return CreatedAtAction("Post", null);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Modifying a consultation reason
        /// </summary>
        /// <param name="id">id of the consultation reason to modify</param>
        /// <param name="reason">the consultation reason with change</param>
        /// <response code="200">consultation reason successfully modified</response>
        /// <response code="204">consultation reason successfully modified (no response body data)</response>
        /// <response code="400">invalid Model, bad request</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // PUT api/<ReasonController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReasonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReasonDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] ReasonDTO reason)
        {
            if (ModelState.IsValid)
            {
                await _reasonService.Edit(id, reason);

                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// Delete a consultation reason
        /// </summary>
        /// <param name="id">id of the consultation reason to delete</param>
        /// <response code="200">consultation reason successfully deleted</response>
        /// <response code="204">consultation reason successfully deleted (no response body data)</response>
        /// <response code="400">Bad Request (client side): consultation reason has been already deleted</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // DELETE api/<ReasonController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ReasonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReasonDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            ReasonDTO reason = await _reasonService.GetById(id);

            if (reason == null)
            {
                return NotFound();
            }

            await _reasonService.Delete(id);

            return NoContent();
        }
    }
}
