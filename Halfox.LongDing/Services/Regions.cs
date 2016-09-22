using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Halfox.Core;


namespace Halfox.LongDing.Services
{
    /// <summary>
    /// 区域操作管理类
    /// </summary>
    public partial class Regions
    {
        static halfoxEntities db;

        public Regions()
        {
            db = new halfoxEntities();
        }

        /// <summary>
        /// 获得全部区域
        /// </summary>
        /// <returns></returns>
        public static List<ha_regions> GetAllRegion()
        {
            return db.ha_regions.ToList();
        }

        /// <summary>
        /// 获得省列表
        /// </summary>
        /// <returns></returns>
        public static List<ha_regions> GetProvinceList()
        {
            List<ha_regions> provinceList = Halfox.Core.HaCache.Get(CacheKeys.MALL_REGION_CHILDLIST + 0) as List<ha_regions>;
            if (provinceList == null)
            {
                provinceList = GetRegionList(0);
                Halfox.Core.HaCache.Insert(CacheKeys.MALL_REGION_CHILDLIST + 0, provinceList);
            }
            return provinceList;
        }

        /// <summary>
        /// 获得市列表
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public static List<ha_regions> GetCityList(int provinceId)
        {
            List<ha_regions> cityList = Halfox.Core.HaCache.Get(CacheKeys.MALL_REGION_CHILDLIST + provinceId) as List<ha_regions>;
            if (cityList == null)
            {
                cityList = GetRegionList(provinceId);
                Halfox.Core.HaCache.Insert(CacheKeys.MALL_REGION_CHILDLIST + provinceId, cityList);
            }
            return cityList;
        }

        /// <summary>
        /// 获得县或区列表
        /// </summary>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public static List<ha_regions> GetCountyList(int cityId)
        {
            List<ha_regions> countyList = Halfox.Core.HaCache.Get(CacheKeys.MALL_REGION_CHILDLIST + cityId) as List<ha_regions>;
            if (countyList == null)
            {
                countyList = GetRegionList(cityId);
                Halfox.Core.HaCache.Insert(CacheKeys.MALL_REGION_CHILDLIST + cityId, countyList);
            }
            return countyList;
        }

        /// <summary>
        /// 获得区域列表
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <returns></returns>
        public static List<ha_regions> GetRegionList(int parentId)
        {
            return db.ha_regions.Where(p => p.parentid == parentId).ToList();
        }

        /// <summary>
        /// 获得区域
        /// </summary>
        /// <param name="regionId">区域id</param>
        /// <returns></returns>
        public static ha_regions GetRegionById(int regionId)
        {
            if (regionId > 0)
            {
                ha_regions regionInfo = Halfox.Core.HaCache.Get(CacheKeys.MALL_REGION_INFOBYID + regionId) as ha_regions;
                if (regionInfo == null)
                {
                    regionInfo = db.ha_regions.Find(regionId);
                    Halfox.Core.HaCache.Insert(CacheKeys.MALL_REGION_INFOBYID + regionId, regionInfo);
                }
                return regionInfo;
            }
            return null;
        }

        /// <summary>
        /// 获得区域
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="layer">级别</param>
        /// <returns></returns>
        public static ha_regions GetRegionByNameAndLayer(string name, int layer)
        {
            ha_regions regionInfo = Halfox.Core.HaCache.Get(string.Format(CacheKeys.MALL_REGION_INFOBYNAMEANDLAYER, name, layer)) as ha_regions;
            if (regionInfo == null)
            {
                regionInfo = db.ha_regions.Where(p => p.name == name && p.layer == layer).FirstOrDefault();
                Halfox.Core.HaCache.Insert(string.Format(CacheKeys.MALL_REGION_INFOBYNAMEANDLAYER, name, layer), regionInfo);
            }
            return regionInfo;
        }

        /// <summary>
        /// 获取IP对应区域
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static ha_regions GetRegionByIP(string ip)
        {
            ha_regions regionInfo = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["loc"];
            if (cookie != null)
            {
                if (cookie["ip"] == ip)
                {
                    regionInfo = GetRegionById(TypeHelper.StringToInt(cookie["regionid"]));
                }
                else
                {
                    cookie.Values["ip"] = ip;
                    regionInfo = IPSearch.SearchRegion(ip);
                    if (regionInfo != null)
                        cookie.Values["regionid"] = regionInfo.regionid.ToString();
                    else
                        cookie.Values["regionid"] = "-1";
                    cookie.Expires = DateTime.Now.AddYears(1);

                    HttpContext.Current.Response.AppendCookie(cookie);
                }

            }
            else
            {
                cookie = new HttpCookie("loc");
                cookie.Values["ip"] = ip;
                regionInfo = IPSearch.SearchRegion(ip);
                if (regionInfo != null)
                    cookie.Values["regionid"] = regionInfo.regionid.ToString();
                else
                    cookie.Values["regionid"] = "-1";
                cookie.Expires = DateTime.Now.AddYears(1);

                HttpContext.Current.Response.AppendCookie(cookie);
            }

            if (regionInfo != null)
                return regionInfo;
            else
                return new ha_regions() { regionid = -1, name = "未知区域" };
        }
    }
}
