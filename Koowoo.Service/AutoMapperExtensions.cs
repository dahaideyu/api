using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Koowoo.Pojo;
using Koowoo.Domain;
using Koowoo.Domain.System;
using Koowoo.Pojo.System;

namespace Koowoo.Services
{
    public static class AutoMapperExtensions
    {
        #region 公用方法

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        #endregion

        #region config

        public static ConfigDto ToModel(this ConfigEntity entity)
        {
            return entity.MapTo<ConfigEntity, ConfigDto>();
        }

        public static ConfigEntity ToEntity(this ConfigDto model)
        {
            return model.MapTo<ConfigDto, ConfigEntity>();
        }

        public static ConfigEntity ToEntity(this ConfigDto model, ConfigEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 字典

        public static DictDto ToModel(this DictEntity entity)
        {
            return entity.MapTo<DictEntity, DictDto>();
        }

        public static DictEntity ToEntity(this DictDto model)
        {
            return model.MapTo<DictDto, DictEntity>();
        }

        public static DictEntity ToEntity(this DictDto model, DictEntity destination)
        {
            return model.MapTo(destination);
        }

        public static DictTypeDto ToModel(this DictTypeEntity entity)
        {
            return entity.MapTo<DictTypeEntity, DictTypeDto>();
        }

        public static DictTypeEntity ToEntity(this DictTypeDto model)
        {
            return model.MapTo<DictTypeDto, DictTypeEntity>();
        }

        public static DictTypeEntity ToEntity(this DictTypeDto model, DictTypeEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion        

        #region 菜单

        public static MenuDto ToModel(this MenuEntity entity)
        {
            return entity.MapTo<MenuEntity, MenuDto>();
        }

        public static MenuEntity ToEntity(this MenuDto model)
        {
            return model.MapTo<MenuDto, MenuEntity>();
        }

        public static MenuEntity ToEntity(this MenuDto model, MenuEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 角色

        public static RoleDto ToModel(this RoleEntity entity)
        {
            return entity.MapTo<RoleEntity, RoleDto>();
        }

        public static RoleEntity ToEntity(this RoleDto model)
        {
            return model.MapTo<RoleDto, RoleEntity>();
        }

        public static RoleEntity ToEntity(this RoleDto model, RoleEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 用户

        public static UserDto ToModel(this UserEntity entity)
        {
            return entity.MapTo<UserEntity, UserDto>();
        }

        public static UserEntity ToEntity(this UserDto model)
        {
            return model.MapTo<UserDto, UserEntity>();
        }

        public static UserEntity ToEntity(this UserDto model, UserEntity destination)
        {
            return model.MapTo(destination);
        }


        public static UserTokenDto ToModel(this UserTokenEntity entity)
        {
            return entity.MapTo<UserTokenEntity, UserTokenDto>();
        }

        public static UserTokenEntity ToEntity(this UserTokenDto model)
        {
            return model.MapTo<UserTokenDto, UserTokenEntity>();
        }

        public static UserTokenEntity ToEntity(this UserTokenDto model, UserTokenEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 梯控配置信息

        public static EvevatorConfigDto ToModel(this EvevatorConfigEntity entity)
        {
            return entity.MapTo<EvevatorConfigEntity, EvevatorConfigDto>();
        }

        public static EvevatorConfigEntity ToEntity(this EvevatorConfigDto model)
        {
            return model.MapTo<EvevatorConfigDto, EvevatorConfigEntity>();
        }

        public static EvevatorConfigEntity ToEntity(this EvevatorConfigDto model, EvevatorConfigEntity destination)
        {
            return model.MapTo(destination);
        }



        #endregion
        #region 区域信息

        public static AreaDto ToModel(this AreaEntity entity)
        {
            return entity.MapTo<AreaEntity, AreaDto>();
        }

        public static AreaEntity ToEntity(this AreaDto model)
        {
            return model.MapTo<AreaDto, AreaEntity>();
        }

        public static AreaEntity ToEntity(this AreaDto model, AreaEntity destination)
        {
            return model.MapTo(destination);
        }



        #endregion

        #region 设备报警

        public static DeviceAlarmDto ToModel(this DeviceAlarmEntity entity)
        {
            return entity.MapTo<DeviceAlarmEntity, DeviceAlarmDto>();
        }

        public static DeviceAlarmEntity ToEntity(this DeviceAlarmDto model)
        {
            return model.MapTo<DeviceAlarmDto, DeviceAlarmEntity>();
        }

        public static DeviceAlarmEntity ToEntity(this DeviceAlarmDto model, DeviceAlarmEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion


        #region 设备

        public static DeviceDto ToModel(this DeviceEntity entity)
        {
            return entity.MapTo<DeviceEntity, DeviceDto>();
        }

        public static DeviceEntity ToEntity(this DeviceDto model)
        {
            return model.MapTo<DeviceDto, DeviceEntity>();
        }

        public static DeviceEntity ToEntity(this DeviceDto model, DeviceEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 设备状态

        public static DeviceStatusDto ToModel(this DeviceStatusEntity entity)
        {
            return entity.MapTo<DeviceStatusEntity, DeviceStatusDto>();
        }

        public static DeviceStatusEntity ToEntity(this DeviceStatusDto model)
        {
            return model.MapTo<DeviceStatusDto, DeviceStatusEntity>();
        }

        public static DeviceStatusEntity ToEntity(this DeviceStatusDto model, DeviceStatusEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 门

        public static DoorDto ToModel(this DoorEntity entity)
        {
            return entity.MapTo<DoorEntity, DoorDto>();
        }

        public static DoorEntity ToEntity(this DoorDto model)
        {
            return model.MapTo<DoorDto, DoorEntity>();
        }

        public static DoorEntity ToEntity(this DoorDto model, DoorEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 人员信息

        public static PersonDto ToModel(this PersonEntity entity)
        {
            return entity.MapTo<PersonEntity, PersonDto>();
        }

        public static PersonEntity ToEntity(this PersonDto model)
        {
            return model.MapTo<PersonDto, PersonEntity>();
        }

        public static PersonEntity ToEntity(this PersonDto model, PersonEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 租户扩展信息

        public static RenterDto ToModel(this RenterEntity entity)
        {
            return entity.MapTo<RenterEntity, RenterDto>();
        }

        public static RenterEntity ToEntity(this RenterDto model)
        {
            return model.MapTo<RenterDto, RenterEntity>();
        }

        public static RenterEntity ToEntity(this RenterDto model, RenterEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 房间

        public static RoomDto ToModel(this RoomEntity entity)
        {
            return entity.MapTo<RoomEntity, RoomDto>();
        }

        public static RoomEntity ToEntity(this RoomDto model)
        {
            return model.MapTo<RoomDto, RoomEntity>();
        }

        public static RoomEntity ToEntity(this RoomDto model, RoomEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 合同

        public static RentalContractDto ToModel(this RentalContractEntity entity)
        {
            return entity.MapTo<RentalContractEntity, RentalContractDto>();
        }

        public static RentalContractEntity ToEntity(this RentalContractDto model)
        {
            return model.MapTo<RentalContractDto, RentalContractEntity>();
        }

        public static RentalContractEntity ToEntity(this RentalContractDto model, RentalContractEntity destination)
        {
            return model.MapTo(destination);
        }

        #endregion

       

        


        
    }
}
