using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using USTA.Model;
using USTA.Common;

namespace USTA.Dal
{
    public class DalOperationAboutProject
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
        public DalOperationAboutProject() {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        #region
        /// <summary>
        /// 添加一个项目 
        /// </summary>
        /// <param name="project"></param>
        public void AddProject(Project project)
        {
            try
            {
                string commandString = "INSERT INTO usta_Project(name, categoryId, userName, userNo, memo, createdTime) VALUES(@name, @categoryId, @userName, @userNo, @memo, @createdTime)";

                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", project.name),
                new SqlParameter("@categoryId", project.category.id),
                new SqlParameter("@userName", project.userName),
                new SqlParameter("@userNo", project.userNo),
                new SqlParameter("@memo", project.memo),
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
        /// 删除一个项目
        /// </summary>
        /// <param name="projectId"></param>
        public void DelProject(int projectId) 
        {
            try
            {
                string commandString = "DELETE FROM usta_Project WHERE id = @projectId";
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@projectId", projectId)
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
        /// 更新一个项目
        /// </summary>
        /// <param name="project"></param>
        public void UpdateProject(Project project)
        {
            try
            {
                string commandString = "UPDATE usta_Project SET name = @name, categoryId = @categoryId, userName = @userName, userNo = @userNo, memo = @memo WHERE id = @projectId";

                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@name", project.name),
                new SqlParameter("@categoryId", project.category.id),
                new SqlParameter("@userName", project.userName),
                new SqlParameter("@userNo", project.userNo),
                new SqlParameter("@memo", project.memo),
                new SqlParameter("@projectId", project.id)

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
        /// 根据id获得一个项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Project GetProject(int projectId) 
        {
            Project project = null;
            try
            {
                List<Project> projects = new List<Project>();
                string commandString = "SELECT usta_Project.*, usta_ProjectCategory.name as categoryName FROM usta_Project, usta_ProjectCategory WHERE usta_Project.id = @projectId AND usta_Project.categoryId = usta_ProjectCategory.id";
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@projectId", projectId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                
                BuildProjects(reader, projects);
                reader.Close();
                if (projects != null && projects.Count != 0)
                {
                    project = projects[0];
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
            return project;

        }
        /// <summary>
        /// 查询所有项目
        /// </summary>
        /// <returns></returns>
        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();
            try
            {
                string commandString = "SELECT usta_Project.*, usta_ProjectCategory.name as categoryName FROM usta_Project, usta_ProjectCategory WHERE usta_Project.categoryId = usta_ProjectCategory.id ORDER BY createdTime DESC";
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);


                BuildProjects(reader, projects);
                reader.Close();
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
            return projects;

        }

        /// <summary>
        /// 根据项目负责人id查询其负责的项目
        /// </summary>
        /// <param name="userNo"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public List<Project> GetProjects(string userNo, string projectName) 
        {
            List<Project> projects = new List<Project>();
            try 
            {
                string commandString = "SELECT usta_Project.*, usta_ProjectCategory.name AS categoryName FROM usta_Project, usta_ProjectCategory WHERE usta_Project.categoryId = usta_ProjectCategory.id AND userNo = @userNo ";
                List<SqlParameter> parameterList = new List<SqlParameter>();
                if (!string.IsNullOrWhiteSpace(projectName))
                {
                    commandString += " AND name = @projectName ";
                    parameterList.Add(new SqlParameter("@projectName", projectName.Trim()));
                }



                commandString += " ORDER BY createdTime DESC";
                parameterList.Add(new SqlParameter("@userNo", userNo));


                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());
                BuildProjects(reader, projects);
                reader.Close();
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

            return projects;
        
        }

        /// <summary>
        /// 根据项目类目及负责人查询类目 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="userName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public List<Project> GetPrjects(string categoryIds, string userName, string projectName) 
        {
            List<Project> projects = new List<Project>();
            try 
            {
                string categoryCase = null;
                string userNameCase = null;
                string projectNameCase = null;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                if (!string.IsNullOrWhiteSpace(categoryIds))
                {
                    categoryCase = " categoryId IN (" + categoryIds + ") ";
                }

                if (!string.IsNullOrWhiteSpace(userName))
                {
                    userNameCase = " userName LIKE '%" + @userName + "%' ";
                }

                if (!string.IsNullOrWhiteSpace(projectName))
                {
                    projectNameCase = " usta_Project.name LIKE '%" + projectName + "%' ";
                }

                string commandString = "SELECT usta_Project.*, usta_ProjectCategory.name as categoryName FROM usta_Project, usta_ProjectCategory WHERE  usta_Project.categoryId = usta_ProjectCategory.id ";

                if (categoryCase != null)
                {
                    commandString += (" AND " + categoryCase);
                }

                if (userNameCase != null)
                {
                    commandString += (" AND " + userNameCase);
                }

                if (projectNameCase != null)
                {
                    commandString += (" AND " + projectNameCase);
                }

                commandString += " ORDER BY createdTime DESC";

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameterList.ToArray());

                BuildProjects(reader, projects);
                reader.Close();

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

            return projects;
        
        }

        private void BuildProjects(IDataReader reader, List<Project> projects)
        {
            Project project;
            while (reader.Read()) 
            {
                project = new Project();
                project.id = int.Parse(reader["id"].ToString().Trim());
                project.name = reader["name"].ToString().Trim();
                ProjectCategory category = new ProjectCategory();
                category.id = int.Parse(reader["categoryId"].ToString().Trim());
                category.name = reader["categoryName"].ToString().Trim();
                project.category = category;
                project.userName = reader["userName"].ToString().Trim();
                project.userNo = reader["userNo"].ToString().Trim();
                project.memo = reader["memo"].ToString().Trim();
                project.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());
                projects.Add(project);
            }
        }


        #endregion

    }
}
