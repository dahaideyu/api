using System.Linq;
using AutoMapper;
using Koowoo.Domain;
using Koowoo.Pojo.Enum;
using Koowoo.Pojo;
using Koowoo.Pojo.System;
using Koowoo.Domain.System;

namespace Koowoo.Services
{
    /// <summary>
    /// AutoMapper 自定义扩展配置
    /// </summary>
    public partial class AutoMapperConfiguration
    {
        /// <summary>
        /// AutoMapper 自定义扩展配置
        /// </summary>
        public static void ConfigExt()
        {
            #region 系统
            Mapper.CreateMap<EvevatorConfigDto, EvevatorConfigEntity>();
            Mapper.CreateMap<EvevatorConfigEntity, EvevatorConfigDto>();

            Mapper.CreateMap<ConfigDto, ConfigEntity>();
            Mapper.CreateMap<ConfigEntity, ConfigDto>();

            Mapper.CreateMap<DictDto, DictEntity>();
            Mapper.CreateMap<DictEntity, DictDto>();

            Mapper.CreateMap<DictTypeDto, DictTypeEntity>();
            Mapper.CreateMap<DictTypeEntity, DictTypeDto>();

            Mapper.CreateMap<MenuDto, MenuEntity>()
             .ForMember(dest => dest.CreateTime, mo => mo.Ignore());
            Mapper.CreateMap<MenuEntity, MenuDto>();         

            Mapper.CreateMap<RoleDto, RoleEntity>();
            Mapper.CreateMap<RoleEntity, RoleDto>();

            Mapper.CreateMap<UserDto, UserEntity>();
            Mapper.CreateMap<UserEntity, UserDto>()
                .ForMember(dest => dest.Password, mo => mo.Ignore());

            Mapper.CreateMap<UserTokenDto, UserTokenEntity>();
            Mapper.CreateMap<UserTokenEntity, UserTokenDto>();
            #endregion
            Mapper.CreateMap<EntryHistoryDto, EntryHistoryEntity>();
            Mapper.CreateMap<EntryHistoryEntity, EntryHistoryDto>();


            Mapper.CreateMap<AreaDto, AreaEntity>()
              .ForMember(dest => dest.CreateTime, mo => mo.Ignore())
              .ForMember(dest => dest.UpdateTime, mo => mo.Ignore());
            Mapper.CreateMap<AreaEntity, AreaDto>();


            Mapper.CreateMap<CardDto, CardEntity>();
            Mapper.CreateMap<CardEntity, CardDto>();

            Mapper.CreateMap<DeviceAlarmDto, DeviceAlarmEntity>();
            Mapper.CreateMap<DeviceAlarmEntity, DeviceAlarmDto>();

            Mapper.CreateMap<DeviceDto, DeviceEntity>()
                 .ForMember(dest => dest.CreateTime, mo => mo.Ignore());
            Mapper.CreateMap<DeviceEntity, DeviceDto>();

            Mapper.CreateMap<DeviceStatusDto, DeviceStatusEntity>();
            Mapper.CreateMap<DeviceStatusEntity, DeviceStatusDto>();

            Mapper.CreateMap<DoorDto, DoorEntity>()
                .ForMember(dest => dest.CreateTime, mo => mo.Ignore());
            Mapper.CreateMap<DoorEntity, DoorDto>();

            Mapper.CreateMap<PersonDto, PersonEntity>()
             .ForMember(dest => dest.CreateTime, mo => mo.Ignore());
            Mapper.CreateMap<PersonEntity, PersonDto>();

            Mapper.CreateMap<RentalContractDto, RentalContractEntity>();
            Mapper.CreateMap<RentalContractEntity, RentalContractDto>();

            Mapper.CreateMap<RenterDto, RenterEntity>()
                .ForMember(dest => dest.CreateTime, mo => mo.Ignore());
            Mapper.CreateMap<RenterEntity, RenterDto>();

            Mapper.CreateMap<RoomDto, RoomEntity>()
               .ForMember(dest => dest.CreateTime, mo => mo.Ignore());
            Mapper.CreateMap<RoomEntity, RoomDto>();

            Mapper.CreateMap<RentalContractDto, RentalContractEntity>();
            Mapper.CreateMap<RentalContractEntity, RentalContractDto>();            

        }
    }
}
