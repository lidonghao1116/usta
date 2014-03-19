using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        AdminRole = 0,
        TeacherRole = 1,
        AssistantRole = 2,
        StudentRole = 3
    }
    /// <summary>
    /// 课程通知类型，作业为0, 一般通知为1
    /// </summary>
    public enum CourseNotifyType
    {
        HomeWork = 0,
        Notice = 1
    }

    /// <summary>
    /// 文件夹类型
    /// </summary>
    public enum FileFolderType
    {
        adminNotify = 0,
        archives = 1,
        bbs = 2,
        courseNotify = 3,
        courseResources = 4,
        experimentResources = 5,
        experiments = 6,
        emailAttachments = 7,
        schoolWorks = 8,
        bbsAvatar = 9,
        remarkExperimentsAndSchoolWorks = 10,
        englishExam = 11,
        gradeCheck = 12,
        //抽签功能附件
        draw = 13
    }

    /// <summary>
    /// 邮件类型
    /// </summary>
    public enum EmailType
    {
        //系统管理员发送邮件
        adminEmail = 0,
        //通知相关邮件
        notifyEmail = 1,
        //成绩审核通知邮件
        gradeCheckNotifyEmail = 2,
        //重修重考通知邮件
        gradeCheckApplyEmail = 3,
        //四六级通知邮件
        englishExamEmail = 4,
        //结课资料上传通知邮件
        archivesEmail = 5,
        //酬金通知邮件
        salaryEmail = 6,
        //工作量通知邮件
        normEmail = 7
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FeedBackType
    {
        /// <summary>
        /// 系统反馈
        /// </summary>
        SystemType = 1,
        /// <summary>
        /// 四六级报名反馈
        /// </summary>
        CET46Type = 2,
        /// <summary>
        /// 成绩审核反馈
        /// </summary>
        EXAMType = 3
    }
}