# .net core通用定时作业

参见：https://www.cnblogs.com/spritekuang/p/13812483.html

## 使用
  1. 执行99_document\作业数据库设计.sql数据库脚本
  2. 修改02_component\CK.Sprite.Job\CK.Sprite.JobWebHost\appsettings.json中的数据库配置信息
  3. 运行02_component\CK.Sprite.Job\CK.Sprite.JobWebHost
  4. 调用swagger中相关接口，job实时生效  
  eg:   
/api/job/JobManager/AddJob  
`{
    "jobGroup": "Test",
    "description": "测试简单job，每10秒执行一次",
    "triggerType": 1,
    "startTime": null,
    "endTime": null,
    "cronConfig": null,
    "simpleIntervalUnit": 1,
    "simpleIntervalValue": 10,
    "execCount": null,
    "priority": null,
    "isActive": true,
    "holidayCalendarId": null,
    "jobName": "simple test",
    "params": null,
    "jobExecType": 2,
    "execLocation": "CK.Sprite.JobBusiness.TestExecJob,CK.Sprite.JobBusiness"
  }`    
  /api/job/JobManager/AddJob  
  `{
    "jobGroup": "Cooperate",
    "description": "cron job，每天凌晨2点执行前一天实际问题汇总数据",
    "triggerType": 2,
    "startTime": null,
    "endTime": null,
    "cronConfig": "0 0 2 * * ?",
    "simpleIntervalUnit": 3,
    "simpleIntervalValue": 1,
    "execCount": null,
    "priority": null,
    "isActive": true,
    "holidayCalendarId": null,
    "jobName": "Quality Daily Job",
    "params": null,
    "jobExecType": 2,
    "execLocation": "CK.Sprite.JobBusiness.TestExecJob,CK.Sprite.JobBusiness"
  }`
