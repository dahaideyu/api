using Koowoo.Pojo;
using Koowoo.Services;
using Koowoo.Services.Auth;
using Koowoo.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Koowoo.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IRoomService roomService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IDoorService doorService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDeviceService deviceService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICardService cardService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IAreaService areaService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRoomUserService roomUserService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICardAuthService cardAuthService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IManageCardService manageCardService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRoomUserCardService userCardService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPersonService personService { get; set; }

        /// <summary>
        /// 房间授权
        /// </summary>
        /// <param name="dto">dto RoomUUID(房间号） AuthType 1 授权，0 取消授权</param>
        /// <returns></returns>
        [HttpPost, Route("room")] //, RequestAuthorize("admin:room:auth")
        public IHttpActionResult RoomAuth([FromBody] AuthDto dto)
        {
            if (dto == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "参数不能为空",
                });
            }

            var card = cardService.GetCardById(dto.CardUUID);
            if (card == null)
            {
                return Ok(new
                {
                    code = 2,
                    msg = "卡信息不存在"
                });
            }


            var roomUser = roomUserService.GetById(dto.RoomUserUUID);
            if (roomUser == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "住户信息不存在"
                });
            }

            var room = roomService.GetById(dto.RoomUUID);
            if (room == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "房间数据不存在"
                });
            }

            var roomAuthDto = new RoomAuthDto();
            roomAuthDto.RoomOtherCode = room.OtherCode;
            roomAuthDto.RoomFloor = room.Floor;
            roomAuthDto.AreaOtherCode = "";
            roomAuthDto.ValidFrom = roomUser.LiveDate;
            roomAuthDto.ValidTo = roomUser.LeaveDate;


            if (dto.AuthType == 0)
            {
                var cardAuth = cardAuthService.GetAuthList(dto.CardUUID, "Room");

                var flag = cardService.DeleteCardAuth(dto.CardUUID, "Room");
                if (flag)
                {
                    return Ok(new
                    {
                        code = 0,
                        msg = "房间销权成功",
                        data = cardAuth != null && cardAuth.Count > 0 ? roomAuthDto : null
                    });
                }
                else
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "房间销权失败"
                    });
                }
            }
            else
            {
                var authList = new List<CardAuthDto>();

                var cardAuthDto = new CardAuthDto
                {
                    CardAuthUUID = Guid.NewGuid().ToString("N"),
                    AuthType = "Room",
                    CardUUID = dto.CardUUID,
                    ValidFrom = roomUser.LiveDate,
                    ValidTo = roomUser.LeaveDate,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    CreateBy = dto.CreateBy                    
                };

                authList.Add(cardAuthDto);

                if (cardService.SaveCardAuth(dto.CardUUID, authList, "Room"))
                {
                    return Ok(new
                    {
                        code = 0,
                        msg = "success！",
                        data = roomAuthDto
                    });
                }
                else
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "添加门授权失败"
                    });
                }
            }
        }


        /// <summary>
        /// 房间授权
        /// </summary>
        /// <param name="dto">dto RoomUUID(房间号） AuthType 1 授权，0 取消授权</param>
        /// <returns></returns>
        [HttpPost, Route("door")] //, RequestAuthorize("admin:door:auth")
        public IHttpActionResult DoorAuth([FromBody] AuthDto dto)
        {
            if (dto == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "参数不能为空"
                });
            }

            var room = roomService.GetById(dto.RoomUUID);
            if (room == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "房间数据不存在"
                });
            }
            var homeNo = string.Format("{0}-{1}", room.HomeNo, room.OtherCode);

            if (dto.AuthType == 0)
            {
                var cardAuth = cardAuthService.GetAuthList(dto.CardUUID, "Door");
                var deviceList = new List<AuthDeviceDto>();

                foreach (var item in cardAuth)
                {
                    var door = doorService.GetById(item.DoorUUID);

                    if (door != null)
                    {
                        var device = deviceService.GetById(door.DeviceUUID);
                        var authDevice = new AuthDeviceDto
                        {
                            DoorNo = door.DoorNo,
                            IP = device.IPAddress,
                            Port = device.Port,
                            SNNumber = device.SNNumber,
                            DeviceType = device.DeviceType.HasValue ? device.DeviceType.Value : 0,
                            CardConvertType = device.CardConvertType,
                            HomeNo = homeNo
                        };
                        deviceList.Add(authDevice);
                    }
                }


                var flag = cardService.DeleteCardAuth(dto.CardUUID, "Door");
                if (flag)
                {
                    return Ok(new
                    {
                        code = 0,
                        msg = "success！",
                        data = cardAuth != null && cardAuth.Count > 0 ? deviceList : null
                    });
                }
                else
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "销权失败"
                    });
                }
            }
            else
            {
                var card = cardService.GetCardById(dto.CardUUID);
                if (card == null)
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "卡信息不存在"
                    });
                }

                var roomUser = roomUserService.GetById(dto.RoomUserUUID);
                if (roomUser == null)
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "住户信息不存在"
                    });
                }
                               
                var area = areaService.GetById(room.AreaUUID);
                if (area == null)
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "区域数据不存在"
                    });
                }

                //根据住户的区域网上查找一层获取单元区域ID
                var dyArea = areaService.GetDtoByCode(area.ParentCode);

                //根据入住的房间取到单元  ，通过单元和房间楼层获取门
                var dyDoorList = doorService.GetDoorListBy(dyArea.AreaUUID, room.Floor);

                //通过单元找到栋（幢）

                var zhuangArea = areaService.GetDtoByCode(dyArea.ParentCode);
                // 获取到幢的门
                var zDoorList = doorService.GetDoorListBy(zhuangArea.AreaUUID, 0);

                var allDoor = dyDoorList.Union(zDoorList).ToList();

                var authList = new List<CardAuthDto>();
                var deviceList = new List<AuthDeviceDto>();
                var convertList = new List<CardNoConvertDto>();

                if (allDoor != null && allDoor.Count > 0)
                {
                    foreach (var door in allDoor)
                    {
                        var cardAuthDto = new CardAuthDto
                        {
                            CardAuthUUID = Guid.NewGuid().ToString().Replace("-", ""),
                            AuthType = "Door",
                            CardUUID = dto.CardUUID,
                            DeviceUUID = door.DeviceUUID,
                            DoorUUID = door.DoorUUID,
                            ValidFrom = roomUser.LiveDate,
                            ValidTo = roomUser.LeaveDate,
                            CreateTime = DateTime.Now
                        };
                        cardAuthDto.UpdateTime = cardAuthDto.CreateTime;
                        cardAuthDto.CreateBy = dto.CreateBy;
                        authList.Add(cardAuthDto);

                        var authDevice = new AuthDeviceDto
                        {
                            DoorNo = door.DoorNo,
                            IP = door.DeviceIP,
                            Port = door.DevicePort,
                            SNNumber = door.DeviceSN,
                            DeviceType = door.DeviceType,
                            CardConvertType=door.CardConvertType,
                            HomeNo = homeNo
                        };
                        deviceList.Add(authDevice);

                        //var convertDto = new CardNoConvertDto
                        //{
                        //    CardNOConverUUID = Guid.NewGuid().ToString("N"),
                        //    CardUUID = dto.CardUUID,
                        //    CardConvertType = door.CardConvertType,
                        //    CardConvertNo = CardNoConvert.ConvertCardNo(card.CardNo, door.CardConvertType, card.CardType)
                        //};
                        //convertList.Add(convertDto);
                    }
                }


                if (cardService.SaveCardAuth(dto.CardUUID, authList, "Door"))
                {
                    return Ok(new
                    {
                        code = 0,
                        msg = "success！",
                        data = deviceList
                    });
                }
                else
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "添加授权失败"
                    });
                }
            }
        }

      
        /// <summary>
        /// 管理人员办卡
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveManageCard")] //, RequestAuthorize("admin:door:auth")
        public IHttpActionResult SaveManageCardAuth([FromBody] ManageCardAuthDto dto)
        {
           // int UserId = CurrentUserUtils.CurrentPrincipal.UserId;


            if (dto == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "参数不能为空"
                });
            }

            var card = cardService.GetCardById(dto.CardUUID);
            if (card == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "卡信息不存在"
                });
            }

            var person = personService.GetById(dto.PersonUUID);
            if (person == null)
            {
                return Ok(new
                {
                    code = 1,
                    msg = "用户信息存在"
                });
            }

            //  var homeNo = string.Format("{0}-{1}", room.HomeNo, room.OtherCode);

            // var communitys = areaService.GetCommunityTree(dto.CommunityUUID);
            //  var homeNos = roomService.GetHomeNos(dto.CommunityUUID);

            if (dto.AuthType == 0)  // 销权
            {
                var cardAuth = cardAuthService.GetAuthList(dto.CardUUID, "Door");
                var deviceList = new List<AuthDeviceDto>();

                foreach (var item in cardAuth)
                {
                    var door = doorService.GetById(item.DoorUUID);

                    if (door != null)
                    {
                        var device = deviceService.GetById(door.DeviceUUID);
                        var homeNo = roomService.GetHomeNoByAreaId(door.AreaUUID);
                        var authDevice = new AuthDeviceDto
                        {
                            DoorNo = door.DoorNo,
                            IP = device.IPAddress,
                            Port = device.Port,
                            SNNumber = device.SNNumber,
                            DeviceType = device.DeviceType.HasValue ? device.DeviceType.Value : 0,
                            CardConvertType = device.CardConvertType,
                            HomeNo = homeNo + "-0000"
                        };
                        deviceList.Add(authDevice);
                    }
                }


                var flag = cardService.DeleteCardAuth(dto.CardUUID, "Door");
                if (flag)
                {
                    return Ok(new
                    {
                        code = 0,
                        msg = "success！",
                        data = deviceList
                    });
                }
                else
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "销权失败"
                    });
                }
            }
            else
            {
                //获取所有门
                var doorList = doorService.GetListByDoorIds(dto.DoorUUIDList);

                if (doorList != null && doorList.Count > 0)
                {
                    var authList = new List<CardAuthDto>();
                    var deviceList = new List<AuthDeviceDto>();
                    
                    foreach (var door in doorList)
                    {
                        var cardAuthDto = new CardAuthDto
                        {
                            CardAuthUUID = Guid.NewGuid().ToString().Replace("-", ""),
                            CardUUID = dto.CardUUID,
                            AuthType = "Door",
                            DoorUUID = door.DoorUUID,
                            ValidFrom = dto.LiveDate,
                            ValidTo = dto.LeaveDate,
                            CreateTime = DateTime.Now
                        };
                        cardAuthDto.UpdateTime = cardAuthDto.CreateTime;
                        //    cardAuthDto.CreateBy = UserId;
                        authList.Add(cardAuthDto);


                        var homeNo = roomService.GetHomeNoByAreaId(door.AreaUUID);
                        var authDevice = new AuthDeviceDto
                        {
                            DoorNo = door.DoorNo,
                            DoorUUID = door.DoorUUID,
                            DoorName = door.DoorName,
                            Floor = door.Floor,
                            IP = door.DeviceIP,
                            Port = door.DevicePort,
                            SNNumber = door.DeviceSN,
                            DeviceType = door.DeviceType,
                            CardConvertType = door.CardConvertType,
                            HomeNo = homeNo + "-0000"
                        };
                        deviceList.Add(authDevice);

                        
                    }


                    if (cardService.SaveCardAuth(dto.CardUUID, authList, "Door"))
                    {
                        return Ok(new
                        {
                            code = 0,
                            msg = "success！",
                            data = deviceList
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            code = 1,
                            msg = "添加授权失败"
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        code = 1,
                        msg = "授权列表为空"
                    });
                }               
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="communityUUID"></param>
        /// <returns></returns>
        [HttpGet, Route("alldoors")] //, RequestAuthorize("admin:door:auth")
        public IHttpActionResult AllDoors(string communityUUID)       {


            //获取所有门
            var doorList = doorService.GetDoorListBy("", 0, communityUUID);



            if (doorList != null && doorList.Count > 0)
            {
                var authList = new List<CardAuthDto>();
                var deviceList = new List<AuthDeviceDto>();
                var convertList = new List<CardNoConvertDto>();

                foreach (var door in doorList)
                {
                    var homeNo = roomService.GetHomeNoByAreaId(door.AreaUUID);
                    var authDevice = new AuthDeviceDto
                    {
                        DoorNo = door.DoorNo,
                        DoorUUID = door.DoorUUID,
                        DoorName = door.DoorName,
                        Floor = door.Floor,
                        IP = door.DeviceIP,
                        Port = door.DevicePort,
                        SNNumber = door.DeviceSN,
                        DeviceType = door.DeviceType,
                        CardConvertType = door.CardConvertType,
                        HomeNo = homeNo + "-0000",
                        Status=door.Status,
                        DoorTypeName=door.DoorTypeName
                    };
                    deviceList.Add(authDevice);
                }
                return Ok(new
                {
                    code = 0,
                    msg = "success！",
                    data = deviceList
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    msg = "没有找到设备列表"
                });
            }
        }

        /// <summary>
        /// 根据个人ID获取授权设备列表
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("device")] //, RequestAuthorize("sys:auth:device")
        [AllowAnonymous]
        public IHttpActionResult DoorList(string personId)
        {
            //获取入住信息
            var roomUser = roomUserService.GetByPersonId(personId);
            var deviceList = new List<RoomUserCardDeviceListDto>();
            if (roomUser != null)
            {
                var cardIds = userCardService.GetUserCardsByUserID(roomUser.RoomUserUUID);
                var authList = cardAuthService.GetCardAuthList(cardIds);

                foreach (var auth in authList)
                {
                    DoorDto doorDto = new DoorDto();
                    DeviceDto deviceDto = new DeviceDto();
                    if (!string.IsNullOrWhiteSpace(auth.DoorUUID))
                    {
                        doorDto = doorService.GetById(auth.DoorUUID);
                        if (!string.IsNullOrWhiteSpace(doorDto.DeviceUUID))
                        {
                            deviceDto = deviceService.GetById(doorDto.DeviceUUID);
                        }
                    }
                    CardDto cardDto = new CardDto();
                    if (!string.IsNullOrWhiteSpace(auth.CardUUID))
                    {
                        cardDto = cardService.GetCardById(auth.CardUUID);
                    }

                    var dto = new RoomUserCardDeviceListDto
                    {
                        CardNo = cardDto.CardNo,
                        ValidFrom = cardDto.ValidFrom.Value,
                        ValidTo = cardDto.ValidTo.Value,
                        DoorType = doorDto.DoorTypeName,
                        DoorName = doorDto.DoorName,
                        DoorUUID= doorDto.DoorUUID,
                        DeviceType = deviceDto.DeviceTypeName,
                        DeviceName = deviceDto.DeviceName,
                        SNNumber = deviceDto.SNNumber
                    };
                    deviceList.Add(dto);
                }
            }

            return Ok(new
            {
                code = 0,
                data = deviceList
            });
        }

        /// <summary>
        /// 根据个人ID获取授权设备列表
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("Managedevice")] //, RequestAuthorize("sys:auth:device")
        public IHttpActionResult ManageDoorList(string personId,string cardId)
        {
            //获取入住信息
            var manageCard = manageCardService.GetByPersonId(personId);
            var deviceList = new List<RoomUserCardDeviceListDto>();
            if (manageCard != null)
            {
                
                var cardIds = manageCard.Select(s=>s.CardUUID).ToArray();
                if (cardIds==null||!cardIds.Contains(cardId))
                {
                    return Ok(new
                    {
                        code = 1,
                        msg="未找到任何权限信息！",

                        data = deviceList
                    });
                }
                var authList = cardAuthService.GetCardAuthList(cardId);

                foreach (var auth in authList)
                {
                    DoorDto doorDto = new DoorDto();
                    DeviceDto deviceDto = new DeviceDto();
                    if (!string.IsNullOrWhiteSpace(auth.DoorUUID))
                    {
                        doorDto = doorService.GetById(auth.DoorUUID);
                        if (!string.IsNullOrWhiteSpace(doorDto.DeviceUUID))
                        {
                            deviceDto = deviceService.GetById(doorDto.DeviceUUID);
                        }
                    }
                    CardDto cardDto = new CardDto();
                    if (!string.IsNullOrWhiteSpace(auth.CardUUID))
                    {
                        cardDto = cardService.GetCardById(auth.CardUUID);
                    }

                    var dto = new RoomUserCardDeviceListDto
                    {
                        CardNo = cardDto.CardNo,
                        ValidFrom = cardDto.ValidFrom.Value,
                        ValidTo = cardDto.ValidTo.Value,
                        DoorType = doorDto.DoorTypeName,
                        DoorName = doorDto.DoorName,
                        DoorUUID = doorDto.DoorUUID,
                        DeviceType = deviceDto.DeviceTypeName,
                        DeviceName = deviceDto.DeviceName,
                        SNNumber = deviceDto.SNNumber
                    };
                    deviceList.Add(dto);
                }
            }

            return Ok(new
            {
                code = 0,
                data = deviceList
            });
        }
    }
}
