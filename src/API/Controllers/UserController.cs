﻿using ELibrary_UserService.Application.Command;
using ELibrary_UserService.Application.Command.Model;
using ELibrary_UserService.Application.Query;
using ELibrary_UserService.Application.Query.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ELibrary_UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserProvider _userProvider;
    private readonly IUserReadProvider _userReadProvider;

    public UserController(IUserProvider userProvider, IUserReadProvider userReadProvider)
    {
        _userProvider = userProvider;
        _userReadProvider = userReadProvider;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<UserReadModel>))]
    public async Task<ActionResult<List<UserReadModel>>> Get()
    {
        var result = await _userReadProvider.GetUsers();
        return result;
    }

    [HttpGet("ById/{userId}")]
    [ProducesResponseType(200, Type = typeof(UserReadModel))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<ActionResult<UserReadModel>> GetById(string userId)
    {
        var result = await _userReadProvider.GetUserById(userId);
        if (result is null)
            return NotFound("User does not exist");
        return result;
    }

    [HttpGet("ByEmail/{email}")]
    [ProducesResponseType(200, Type = typeof(UserReadModel))]
    [ProducesResponseType(404, Type = typeof(string))]
    public async Task<ActionResult<UserReadModel>> GetByEmail(string email)
    {
        var result = await _userReadProvider.GetUserByEmail(email);
        if (result is null)
            return NotFound("User does not exist");
        return result;
    }

    
    [HttpPatch("{userId}")]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    [SwaggerOperation(Summary = "Modify user data. You can send params which you want to change. Omitted params will remain the same. description with value \"\" will set description to null")]
    public async Task<ActionResult> Patch(string userId, [FromBody] ModifyUserModel data)
    {
        var senderUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var senderIsAdmin = User.IsInRole("admin");
        var senderIsEmployee = User.IsInRole("employee");
        if (senderUserId == userId || senderIsAdmin || senderIsEmployee)
        {
            await _userProvider.ModifyUser(userId, data);
            return NoContent();
        }
        else
            return Forbid();
    }

    
    [HttpPost("{userId}/Pay")]
    [Authorize(Roles = "admin, employee")]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> PostPay(string userId, [FromBody] decimal amount)
    {
        await _userProvider.Pay(userId, amount);
        return NoContent();
    }

    [HttpPost("AddToWatchList/{bookId}")]
    [Authorize]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> PostAddToWatchList(int bookId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _userProvider.AddToWatchList(userId, bookId);
        
        return NoContent();
    }

    [HttpDelete("RemoveFromWatchList/{bookId}")]
    [Authorize]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> RemoveFromWatchList(int bookId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _userProvider.RemoveFromWatchList(userId, bookId);
        
        return NoContent();
    }


    [HttpPost("{userId}/Block")]
    [Authorize(Roles = "admin, employee")]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> BlockUser(string userId)
    {
        await _userProvider.BlockUser(userId);
        return NoContent();
    }

    [HttpPost("{userId}/UnBlock")]
    [Authorize(Roles = "admin, employee")]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> UnblockUser(string userId)
    {
        await _userProvider.UnBlockUser(userId);
        return NoContent();
    }

    [HttpPost("Reaction/{bookId}")]
    [Authorize]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    [SwaggerOperation(Summary = "Add or modify a reaction for a book")]
    public async Task<ActionResult> AddOrModifyReaction(int bookId, [FromQuery] bool like)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _userProvider.AddOrModifyReaction(userId, bookId, like);
        return NoContent();
    }

    [HttpDelete("Reaction/{bookId}")]
    [Authorize]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    public async Task<ActionResult> RemoveReaction(int bookId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _userProvider.RemoveReaction(userId, bookId);
        return NoContent();
    }

    [HttpPost("Review/{bookId}")]
    [Authorize]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    [SwaggerOperation(Summary = "Add or modify a review for a book")]
    public async Task<ActionResult> AddOrModifyReaction(int bookId, [FromBody] string content)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _userProvider.AddOrModifyReview(userId, bookId, content);
        return NoContent();
    }

    [HttpDelete("Review/{bookId}")]
    [Authorize]
    [ProducesResponseType(400, Type = typeof(string))]
    [ProducesResponseType(401, Type = typeof(string))]
    [ProducesResponseType(403, Type = typeof(string))]
    [ProducesResponseType(404, Type = typeof(string))]
    [ProducesResponseType(204)]
    [SwaggerOperation(Summary = "Review's author, employee or admin can remove a review")]
    public async Task<ActionResult> RemoveReview(int bookId, [FromQuery] string userId)
    {
        var senderUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var senderIsAdmin = User.IsInRole("admin");
        var senderIsEmployee = User.IsInRole("employee");
        if (senderUserId == userId || senderIsAdmin || senderIsEmployee)
        {
            await _userProvider.RemoveReview(userId, bookId);
            return NoContent();
        }
        else
            return Forbid();
    }
}
