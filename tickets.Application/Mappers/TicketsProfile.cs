using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Requests.Auth;
using tickets.Domain.Requests.Task;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using UserEntity = tickets.Domain.Entities.User;
using TaskEntity = tickets.Domain.Entities.Task;
using tickets.Domain.Models;
using tickets.Application.Helpers;

namespace tickets.Application.Mappers
{
    public class TicketsProfile : Profile
    {
        public TicketsProfile()
        {
            AllowNullDestinationValues = true;
            AllowNullCollections = true;

            #region Auth
            CreateMap<AuthRegisterRequest, UserEntity>()
                .ForMember(entity => entity.UserName, map => map.MapFrom(root => root.UserName))
                .ForMember(entity => entity.Email, map => map.MapFrom(root => root.Email));

            CreateMap<UserEntity, UserModel>()
                .ForMember(model => model.Id, map => map.MapFrom(entity => entity.Id))
                .ForMember(model => model.UserName, map => map.MapFrom(entity => entity.UserName))
                .ForMember(model => model.Email, map => map.MapFrom(entity => entity.Email));
            #endregion

            #region Task
            CreateMap<TaskCreateRequest, TaskEntity>()
                .ForMember(model => model.Title, map => map.MapFrom(root => root.Title))
                .ForMember(model => model.Content, map => map.MapFrom(root => root.Content))
                .ForMember(model => model.Priority, map => map.MapFrom(root => root.Priority));

            CreateMap<TaskEntity, TaskModel>()
                .ForMember(model => model.Id, map => map.MapFrom(entity => entity.Id))
                .ForMember(model => model.Title, map => map.MapFrom(entity => entity.Title))
                .ForMember(model => model.Content, map => map.MapFrom(entity => entity.Content))
                .ForMember(model => model.Priority, map => map.MapFrom(entity => entity.Priority))
                .ForMember(model => model.CurrentStatus, map => map.MapFrom(entity => entity.CurrentStatus.ToString()))
                .ForMember(model => model.CreatedBy, map => map.MapFrom(entity => entity.CreatedBy.UserName))
                .ForMember(model => model.CreatedOn, map => map.MapFrom(entity => CommonHelper.ConvertDateToString(entity.CreatedOn)));
            #endregion

        }
    }
}
