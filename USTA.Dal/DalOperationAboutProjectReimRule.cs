using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using USTA.Model;
using System.Data;
using USTA.Common;

namespace USTA.Dal
{
    public class DalOperationAboutProjectReimRule
    {
        #region
        /// <summary>
        /// SqlConnection变量
        /// </summary>
        public SqlConnection conn
        {
            set;
            get;
        }
        #endregion

        #region
        /// <summary>
        /// 构造方法
        /// </summary>
        public DalOperationAboutProjectReimRule()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region
        /// <summary>
        /// 添加一条项目报销项规则
        /// </summary>
        /// <param name="rule"></param>
        public void AddProjectReimRule(ProjectReimRule rule) 
        {
            try 
            {
                string commandString = "INSERT INTO usta_ProjectReimRule(projectId, reimId, reimValue, maxReimValue, createdTime) VALUES(@projectId, @reimId, @reimValue, @maxReimValue, @createdTime)";

                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@projectId", rule.project.id),
                new SqlParameter("@reimId", rule.reim.id),
                new SqlParameter("@reimValue", rule.reimValue),
                new SqlParameter("@maxReimValue", rule.maxReimValue),
                new SqlParameter("@createdTime", DateTime.Now)
            };

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// 更新一条项目报销项规则
        /// </summary>
        /// <param name="rule"></param>
        public void UpdateProjectReimRule(ProjectReimRule rule) {

            try
            {
                string commandString = "UPDATE usta_ProjectReimRule SET projectId=@projectId, reimId=@reimId, reimValue = @reimValue, maxReimValue = @maxReimValue WHERE ruleId = @ruleId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ruleId", rule.ruleId),
                new SqlParameter("@projectId", rule.project.id),
                new SqlParameter("@reimId", rule.reim.id),
                new SqlParameter("@reimValue", rule.reimValue),
                new SqlParameter("@maxReimValue", rule.maxReimValue)
            };

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// 删除一条项目报销项规则
        /// </summary>
        /// <param name="ruleId"></param>
        public void DelProjectReimRule(int ruleId) {

            try
            {
                string commandString = "DELETE FROM usta_ProjectReimRule WHERE ruleId = @ruleId";
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@ruleId", ruleId)
                };

                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, commandString, parameters);
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
            
        }

        public ProjectReimRule GetProjectReimRule(int ruleId)
        {
            ProjectReimRule rule = null;
            try
            {
                List<ProjectReimRule> projectReimRules = new List<ProjectReimRule>();
                string commandString = "SELECT usta_ProjectReimRule.*, usta_Project.name AS projectName, usta_Reim.name AS reimName FROM usta_ProjectReimRule, usta_Project, usta_Reim WHERE ruleId = @ruleId AND usta_ProjectReimRule.projectId = usta_Project.id AND usta_ProjectReimRule.reimId = usta_Reim.id";
                SqlParameter[] parameters = new SqlParameter
                []{
                new SqlParameter("@ruleId", ruleId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                BuildReimRule(reader, projectReimRules);
                reader.Close();
                if (projectReimRules != null && projectReimRules.Count != 0)
                {
                    rule = projectReimRules[0];
                }
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
            return rule;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public ProjectReimRule GetProjectReimRule(int projectId, int reimId)
        {
            ProjectReimRule rule = null;
            try
            {
                List<ProjectReimRule> projectReimRules = new List<ProjectReimRule>();
                string commandString = "SELECT usta_ProjectReimRule.*, usta_Project.name AS projectName, usta_Reim.name AS reimName FROM usta_ProjectReimRule, usta_Project, usta_Reim WHERE projectId = @projectId AND reimId = @reimId AND usta_ProjectReimRule.projectId = usta_Project.id AND usta_ProjectReimRule.reimId = usta_Reim.id";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@projectId", projectId),
                new SqlParameter("@reimId", reimId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                BuildReimRule(reader, projectReimRules);
                reader.Close();
                if (projectReimRules != null && projectReimRules.Count != 0)
                {
                    rule = projectReimRules[0];
                }
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }
            
            return rule;
        }

        /// <summary>
        /// 根据条件查询项目报销规则记录
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectId"></param>
        /// <param name="reimId"></param>
        /// <returns></returns>
        public List<ProjectReimRule> GetProjectReimRules(string projectName, int projectId, int reimId) 
        {
            List<ProjectReimRule> projectReimRules = new List<ProjectReimRule>();

            try
            {
                
                string commandString;
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (string.IsNullOrWhiteSpace(projectName) && reimId == 0 & projectId == 0)
                {
                    commandString = "SELECT usta_ProjectReimRule.*, usta_Project.name AS projectName, usta_Reim.name AS reimName FROM usta_ProjectReimRule, usta_Project, usta_Reim WHERE usta_ProjectReimRule.projectId = usta_Project.id AND usta_ProjectReimRule.reimId = usta_Reim.id ORDER BY usta_ProjectReimRule.createdTime DESC";
                }
                else
                {
                    string projectCase = null;
                    string reimCase = null;

                    if (!string.IsNullOrWhiteSpace(projectName))
                    {
                        projectCase = " usta_Project.name LIKE '%" + projectName + "%' ";
                    }
                    else if (projectId != 0)
                    {
                        projectCase = " usta_ProjectReimRule.projectId = @projectId ";
                        parameters.Add(new SqlParameter("@projectId", projectId));
                    }

                    if (reimId != 0)
                    {

                        reimCase = " usta_ProjectReimRule.reimId = @reimId ";
                        parameters.Add(new SqlParameter("@reimId", reimId));
                    }

                    commandString = "SELECT usta_ProjectReimRule.*, usta_Project.name AS projectName, usta_Reim.name AS reimName FROM usta_ProjectReimRule, usta_Project, usta_Reim WHERE " + (reimCase == null ? "" : (reimCase + " AND ")) + (projectCase == null ? "" : (projectCase + " AND ")) + " usta_ProjectReimRule.projectId = usta_Project.id AND usta_ProjectReimRule.reimId = usta_Reim.id ORDER BY usta_ProjectReimRule.createdTime DESC";
                }

                IDataReader dataReader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters.ToArray());
                BuildReimRule(dataReader, projectReimRules);
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                CommonUtility.RedirectUrl();
            }
            finally
            {
                conn.Close();
            }

            return projectReimRules;
 

        }

        private void BuildReimRule(IDataReader dataReader, List<ProjectReimRule> projectReimRules)
        {
            ProjectReimRule rule;
            while(dataReader.Read()){
                rule = new ProjectReimRule();
                rule.ruleId = int.Parse(dataReader["ruleId"].ToString().Trim());

                rule.project = new Project
                {
                    id = int.Parse(dataReader["projectId"].ToString().Trim()),
                    name = dataReader["projectName"].ToString().Trim()
                };

                rule.reim = new Reim
                {
                    id = int.Parse(dataReader["reimId"].ToString().Trim()),
                    name = dataReader["reimName"].ToString().Trim()
                };

                rule.reimValue = CommonUtility.ConvertFormatedFloat("{0:0.00}", dataReader["reimValue"].ToString().Trim());
                rule.maxReimValue = CommonUtility.ConvertFormatedFloat("{0:0.00}", dataReader["maxReimValue"].ToString().Trim());
                projectReimRules.Add(rule);
            }
        }

        #endregion



    }
}
