using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using USTA.Model;
using USTA.Dal;
using USTA.Bll;
using USTA.Cache;
using System.Web.UI.HtmlControls;
using System.Configuration;
using USTA.Common;
using USTA.PageBase;

public partial class MasterPage_FrameManage : CheckUserWithCommonMasterPageBase
{
    private static string pageName = "Teacher_NormManager";
    private static string salaryPageName = "Teacher_SalaryManage";
    private static string projectPageName = "ProjectManager";
    public bool norm_right = false;
    public bool salary_right = false;
    public bool project_right = false;

    public string systemVersion = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            
            UserCookiesInfo UserCookiesInfo = BllOperationAboutUser.GetUserCookiesInfo();

            //UserCookiesInfo UserCookiesInfo = CacheCollections.GetUserCookiesInfo();

            string nickName = string.Empty;

            switch (UserCookiesInfo.userType)
            {
                case 0:
                    ulAdmin.Visible = true;
                    nickName = "老师";

                    if (UserCookiesInfo.teacherType != null && UserCookiesInfo.teacherType == "本院")
                    {
                        drawAdminTeacher.Visible = true;
                    }

                    break;
                case 1:
                case 2:
                    ulTeacher.Visible = true;
                    nickName = "老师";
                    DalOperationNorm dalOperationNorm = new DalOperationNorm();
                    DalOperationAboutUserAuth dalua = new DalOperationAboutUserAuth();
                    UserAuth auth = dalOperationNorm.GetUserAuth(pageName);
                    UserAuth salaryAuth = dalua.GetUserAuth(salaryPageName);
                    UserAuth projectAuth = dalua.GetUserAuth(projectPageName);

                    if (auth!=null&&auth.userIds != null)
                    {
                        string[] _ids = auth.userIds.Split(',');
                        if (_ids != null)
                        {
                            norm_right = _ids.Contains(UserCookiesInfo.userNo);
                        }
                    }

                    if (salaryAuth!=null&&salaryAuth.userIds != null)
                    {
                        string[] _ids = salaryAuth.userIds.Split(',');
                        if (_ids != null)
                        {
                            if (_ids.Contains(UserCookiesInfo.userNo))
                            {
                                salary_right = true;
                            }
                        }
                    }

                    if (projectAuth != null)
                    {
                        string projectUserIds = projectAuth.userIds;
                        if (!(projectUserIds == null || projectUserIds.Trim().Length == 0))
                        {
                            if (projectUserIds.Contains(UserCookiesInfo.userNo))
                            {
                                project_right = true;
                            }
                        }
                    }

                    if (UserCookiesInfo.teacherType != null && UserCookiesInfo.teacherType == "本院")
                    {
                        drawTeacher.Visible = true;
                    }
                     
                    break;
                case 3:
                    ulStudent.Visible = true;
                    nickName = "同学";
                    break;
                default:
                    break;
            }
            user.InnerHtml = "尊敬的" + UserCookiesInfo.userName.Trim() + nickName + "  您好！";
            //设置系统配置信息

            //DalOperationBaseConfig dobc = new DalOperationBaseConfig();
            BaseConfig baseconfig = CacheCollections.GetBaseConfig();
            Page.Title = baseconfig.systemName + " 当前系统版本号：" + baseconfig.systemVersion;
            systemVersion = baseconfig.systemVersion;
            this.lblCopyRight.Text = baseconfig.systemCopyRight + " 当前系统版本号：" + baseconfig.systemVersion;
            //判断是否为班主任
            DalOperationAboutEnglishExam dalOperationAboutEnglishExam = new DalOperationAboutEnglishExam();
            if (dalOperationAboutEnglishExam.CheckIsHeadTeacherByTeacherNo(UserCookiesInfo.userNo))
            {
                englishExamManage.Visible = true;
            }
        }
    }
}
