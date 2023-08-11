﻿global using SocialMediaApp.Api.Extensions;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Mvc;
global using SocialMediaApp.Api.Registrars;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using SocialMediaApp.Api.Options;
global using System.Text;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using SocialMediaApp.Application.Options;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using MediatR;
global using SocialMediaApp.Application.Enums;
global using SocialMediaApp.Application.Models;
global using SocialMediaApp.Application.Posts.Commands;
global using SocialMediaApp.Data;
global using SocialMediaApp.Domain.Aggregates.PostAggregate;
global using SocialMediaApp.Api.Contracts.Common;
global using SocialMediaApp.Api.Contracts.Post.Requests;
global using SocialMediaApp.Api.Contracts.Post.Responses;
global using SocialMediaApp.Api.Filters;
global using SocialMediaApp.Application.Posts.Queries;
