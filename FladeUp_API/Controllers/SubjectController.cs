﻿using AutoMapper;
using AutoMapper;
using FladeUp_Api.Data.Entities.Identity;
using FladeUp_Api.Data;
using FladeUp_Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FladeUp_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FladeUp_API.Models.Class;
using FladeUp_API.Models.User;
using FladeUp_API.Requests.Class;
using FladeUp_API.Requests.Subject;
using System.Linq;
using FladeUp_API.Models;
using FladeUp_API.Models.Subject;
using System;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FladeUp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        // GET: api/<SubjectController>
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly AppEFContext _appEFContext;
        private readonly ICloudStorageService _cloudStorage;

        public SubjectController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService, IMapper mapper, AppEFContext appEFContext, ICloudStorageService cloudStorage)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            _appEFContext = appEFContext;
            _cloudStorage = cloudStorage;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var subject = _appEFContext.Subjects
                    .Where(u => u.Id == id)
                    .SingleOrDefault();

                return Ok(subject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subjects = await _appEFContext.Subjects
                    .ToListAsync();

                return Ok(new Response<List<SubjectEnitity>>(subjects));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetSubjects([FromQuery] string? searchQuery, [FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var subjects = new List<SubjectEnitity>();
                if(searchQuery != null)
                {
                    subjects = await _appEFContext.Subjects
                    .OrderBy(s => s.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Where(s => s.Name.ToLower().Contains(searchQuery.ToLower()) || s.Color.ToLower().Contains(searchQuery.ToLower()) || s.Id.ToString() == searchQuery)
                    .ToListAsync();

                }

                else
                {
                    subjects = await _appEFContext.Subjects
                    .OrderBy(s => s.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                }

                var totalRecords = await _appEFContext.Subjects.CountAsync();
                var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(pageSize)));

                return Ok(new PagedResponse<List<SubjectEnitity>>(subjects, page, pageSize, totalPages, totalRecords));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SubjectCreateRequest model)
        {
            try
            {
                var subject = new SubjectEnitity()
                {
                    Name = model.Name,
                    Color = model.Color,
                };
                _appEFContext.Add(subject);
                await _appEFContext.SaveChangesAsync();

                return Ok(subject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] SubjectUpdateRequest model)
        {
            try
            {
                var subject = await _appEFContext.Subjects
                     .Where(r => r.Id == model.Id)
                     .SingleOrDefaultAsync();

                if (subject == null)
                    return NotFound();

                subject.Name = model.Name;
                subject.Color = model.Color;

                _appEFContext.Update(subject);
                await _appEFContext.SaveChangesAsync();


                return Ok(subject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var subject = await _appEFContext.Subjects
                    .Where(r => r.Id == id)
                    .SingleOrDefaultAsync();

                if (subject == null)
                    return NotFound();

                _appEFContext.Remove(subject);
                await _appEFContext.SaveChangesAsync();


                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
