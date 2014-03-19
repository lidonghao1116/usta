using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using USTA.Dal;
using USTA.Common;

public partial class Administrator_DataBaseSynchronize : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //控制Tab的显示
        string fragmentFlag = "1";

        if (Request["fragment"] != null)
        {
            fragmentFlag = Request["fragment"];
        }

        CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3
            , divFragment1, divFragment2, divFragment3);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!(chkMajorTypeSchoolClass.Checked || chkCourse.Checked || chkTeacherCoursePlan.Checked
            || chkStudentElectiveStudent.Checked))
        {
            ltlResult.Text = "请选择要同步的数据内容！<br/>";
        }
        else
        {
            ltlResult.Text = string.Empty;
            DalOperationAboutDataSynchronize dalOperationAboutDataSynchronize
                = new DalOperationAboutDataSynchronize();

            //同步学期数据
            dalOperationAboutDataSynchronize.TermTagsDataSynchronize();

            //专业与班级数据同步
            if (chkMajorTypeSchoolClass.Checked)
            {
                dalOperationAboutDataSynchronize.MajorDataSynchronize();
                ltlResult.Text += "专业数据同步成功！<br/>";
                dalOperationAboutDataSynchronize.SchoolClassDataSynchronize();
                ltlResult.Text += "班级数据同步成功！<br/>";
            }

            //课程数据同步
            if (chkCourse.Checked)
            {
                dalOperationAboutDataSynchronize.CourseDataSynchronize();
                ltlResult.Text += "课程数据同步成功！<br/>";
            }

            //学生及选课（作业与实验）数据同步
            if (chkStudentElectiveStudent.Checked)
            {
                dalOperationAboutDataSynchronize.StudentDataSynchronize();
                ltlResult.Text += "学生数据同步成功！<br/>";
                dalOperationAboutDataSynchronize.CoursesElectiveDataSynchronize();
                ltlResult.Text += "学生选课数据同步成功！<br/>";
                dalOperationAboutDataSynchronize.SchoolWorkAndExperimentsDataSynchronize();
                ltlResult.Text += "作业与实验数据同步成功！<br/>";
            }

            //教师及任课数据同步
            if (chkTeacherCoursePlan.Checked)
            {
                dalOperationAboutDataSynchronize.TeacherDataSynchronize();
                ltlResult.Text += "教师任课数据同步成功！<br/>";
                dalOperationAboutDataSynchronize.CoursesTeacherRelationDataSynchronize();
                ltlResult.Text += "教师数据同步成功！<br/>";
            }
        }
    }
}