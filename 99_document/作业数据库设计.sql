CREATE TABLE `JobConfigs` (
  `Id` char(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '主键',
  `JobGroup` varchar(100) NULL COMMENT 'Job分组，为空时，添加到默认组',
  `Description` varchar(1000) NULL COMMENT 'job描述',
  `TriggerType` int NOT NULL COMMENT '触发器类型',
  `StartTime` datetime(6) NULL COMMENT '触发器开始时间，触发器类型为At时，必须指定',
  `EndTime` datetime(6) NULL COMMENT '触发器结束时间',
  `CronConfig` varchar(500) NULL COMMENT 'Cron触发器配置',
  `SimpleIntervalUnit` int NULL COMMENT '简单触发器，循环单位',
  `SimpleIntervalValue` int NULL COMMENT '简单触发器，循环值',
  `ExecCount` int NULL COMMENT '最多执行次数，不设置时，为一直执行',
  `Priority` int NULL COMMENT '优先级(同一时间执行时生效)',
  `IsActive` bit(1) NOT NULL COMMENT '是否激活',
  `HolidayCalendarId` char(36) NULL COMMENT '假期配置Id',
  `JobName` varchar(100) NOT NULL COMMENT 'Job名称',
  `Params` text NULL COMMENT 'Job名称',
  `JobExecType` int NOT NULL COMMENT 'job执行类型',
  `ExecLocation` varchar(500) NOT NULL COMMENT '用地址(api时为url，reflect时为类定义信息)',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `HolidayCalendars` (
  `Id` char(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '主键',
  `Description` varchar(1000) NULL COMMENT 'job描述',
  `Config` text NOT NULL COMMENT '假期值，用;号隔开',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;