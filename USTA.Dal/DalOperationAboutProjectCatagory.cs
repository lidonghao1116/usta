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
    public class DalOperationAboutProjectCategory
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

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public DalOperationAboutProjectCategory() 
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        }
        #endregion

        /// <summary>
        /// 添加项目类目
        /// </summary>
        /// <param name="category"></param>
        public void addProjectCategory(ProjectCategory category)
        {
            try 
            {
                string commandString = "INSERT INTO usta_ProjectCategory (name, categoryLevel, parendId, memo, createdTime) VALUES(@name, @categoryLevel, @parendId, @memo, @createdTime)";

                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", category.name),
                new SqlParameter("@categoryLevel", category.categoryLevel),
                new SqlParameter("@parendId", category.parentId),
                new SqlParameter("@memo", category.memo),
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
        /// 
        /// </summary>
        /// <param name="projectCategory"></param>
        public void UpdateProjectCategory(ProjectCategory projectCategory) 
        {
            try {
                string commandString = "UPDATE usta_ProjectCategory SET name = @name, categoryLevel = @categoryLevel, parendId = @parendId, memo = @memo WHERE id = @id";

                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", projectCategory.name),
                new SqlParameter("@categoryLevel", projectCategory.categoryLevel),
                new SqlParameter("@parendId", projectCategory.parentId),
                new SqlParameter("@memo", projectCategory.memo),
                new SqlParameter("@id", projectCategory.id)
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
        /// 获得所有的项目类目
        /// </summary>
        /// <returns></returns>
        public List<ProjectCategory> GetAllProjectCategory() 
        {
            List<ProjectCategory> categoryList = new List<ProjectCategory>();
            try
            {
                string commandString = "SELECT * FROM usta_ProjectCategory ORDER BY createdTime DESC";
                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString);
                BuildProjectCategory(reader, categoryList);
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
            
            return categoryList;        
        }
        /// <summary>
        /// 根据父类目获得直接子类目
        /// </summary>
        /// <param name="parendId"></param>
        /// <returns></returns>
        public List<ProjectCategory> GetProjectCategoryByParendId(int parendId) 
        {
            List<ProjectCategory> categoryList = new List<ProjectCategory>();
            try
            {
                string commandString = "SELECT * FROM usta_ProjectCategory WHERE parendId = @parendId ORDER BY createdTime DESC";
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@parendId", parendId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                BuildProjectCategory(reader, categoryList);
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
            return categoryList;
        }
        /// <summary>
        /// 根据父类目获得所有的直接和间接子类目
        /// </summary>
        /// <param name="parendId"></param>
        /// <returns></returns>
        public List<ProjectCategory> GetAllProjectCategoryByParendId(int parendId) 
        {   
            List<ProjectCategory> resultList = new List<ProjectCategory>();
            
            List<ProjectCategory> categoryList = GetProjectCategoryByParendId(parendId);
            if (categoryList.Count != 0) 
            {
                resultList.AddRange(categoryList);

                foreach (ProjectCategory category in categoryList) 
                {
                    resultList.AddRange(GetAllProjectCategoryByParendId(category.id));
                }
            }
            return resultList;
        }

        /// <summary>
        /// 根据父类目获得所有的直接或间接叶子上的子类目
        /// </summary>
        /// <param name="parendId"></param>
        /// <returns></returns>
        public List<ProjectCategory> GetAllLastProjectCategoryByParentId(int parendId) 
        {   
            List<ProjectCategory> resultList = new List<ProjectCategory>();

            ProjectCategory projectCategory = GetProjectCategoryById(parendId);
            if (projectCategory != null) {
                GetAllLastProjceCategoryByParendCategory(projectCategory, resultList);
            }
            return resultList;
        }

        private void GetAllLastProjceCategoryByParendCategory(ProjectCategory projectCategory, List<ProjectCategory> resultList) 
        {
            List<ProjectCategory> categoryList = GetProjectCategoryByParendId(projectCategory.id);

            if (categoryList.Count == 0)
            {
                resultList.Add(projectCategory);
            }
            else {
                foreach (ProjectCategory category in categoryList) 
                {
                    GetAllLastProjceCategoryByParendCategory(category, resultList);
                }
            }
        }


       /// <summary>
       /// 根据类目等级获得相应等级的类目
       /// </summary>
       /// <param name="categoryLevel"></param>
       /// <returns></returns>
        public List<ProjectCategory> GetProjectCategoryByLevel(int categoryLevel) 
        {
            List<ProjectCategory> categoryList = new List<ProjectCategory>();
            try
            {  
                string commandString = "SELECT * FROM usta_ProjectCategory WHERE categoryLevel = @categoryLevel ORDER BY createdTime DESC";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@categoryLevel", categoryLevel)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                BuildProjectCategory(reader, categoryList);
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

            return categoryList;
        
        }
        /// <summary>
        /// 根据类目id获得类目详情
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ProjectCategory GetProjectCategoryById(int categoryId) 
        {
            ProjectCategory category = null;
            try
            {
                List<ProjectCategory> categoryList = new List<ProjectCategory>();
                string commandString = "SELECT * FROM usta_ProjectCategory WHERE id = @categoryId";
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@categoryId", categoryId)
            };

                IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.Text, commandString, parameters);
                BuildProjectCategory(reader, categoryList);
                reader.Close();
                if (categoryList.Count != 0)
                {
                    category = categoryList[0];
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
            return category;
        }

        
        private void GetProjectCategoryPathById(int categoryId, List<ProjectCategory> categoryPath) 
        {
            ProjectCategory category = GetProjectCategoryById(categoryId);
            if (category != null) {
                if (category.parentId != 0)
                {
                    GetProjectCategoryPathById(category.parentId, categoryPath);
                    categoryPath.Add(category);
                }
                else {
                    categoryPath.Add(category);
                }
            } 
        }

        /// <summary>
        /// 根据类目id获得类目路径上的类目列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<ProjectCategory> GetProjectCategoryPathById(int categoryId) 
        {   
            List<ProjectCategory> categoryPath = new List<ProjectCategory>();
            GetProjectCategoryPathById(categoryId, categoryPath);

            return categoryPath;
        
        }


        private void BuildProjectCategory(IDataReader reader, List<ProjectCategory> categoryList)
        {
            ProjectCategory category;
            while (reader.Read()) {
                category = new ProjectCategory();
                category.id = int.Parse(reader["id"].ToString().Trim());
                category.name = reader["name"].ToString().Trim();
                category.categoryLevel = int.Parse(reader["categoryLevel"].ToString().Trim());
                category.parentId = int.Parse(reader["parendId"].ToString().Trim());
                category.memo = reader["memo"].ToString().Trim();
                category.createdTime = DateTime.Parse(reader["createdTime"].ToString().Trim());
                categoryList.Add(category);
            }
        }
    }
}
