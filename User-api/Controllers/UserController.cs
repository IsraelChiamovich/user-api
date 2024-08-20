﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_api.Dto;
using User_api.Models;
using User_api.Services;

namespace User_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IJwtService jwtService) : ControllerBase
    {
        [HttpPost("create-token")]
        public ActionResult<string> CreateToken([FromBody] UserModel user)
        {
            return Ok(jwtService.GenerateToken(user));
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserModel>>> GetAll() =>
            Ok(await userService.GetAllUsersAsync());


        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            return (user == null) ? NotFound($"The user by id {id} is not found") : Ok(user);
        }


        [HttpPost("Create")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModel model)
        {
            try
            {
                var user = await userService.CreateUserAsync(model);
                return Created("User created sussecfully", user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, [FromBody] UserModel user)
        {
            try
            {
                var userToUpdate = await userService.UpdateUserAsync(id, user);
                return Ok(userToUpdate);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<ActionResult<UserModel>> DeleteUser(int id)
        {
            try
            {
                return Ok(await userService.DeledeUserAsync(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Auth([FromBody] LoginDto loginDto)
        {
            try
            {
                UserModel authenticad = await userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
                return Ok(jwtService.GenerateToken(authenticad));
                    
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        
    }
}
