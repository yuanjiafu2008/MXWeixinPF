﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MxWeiXinPF.Common;
using System.Text;

namespace MxWeiXinPF.Web.weixin.ucard
{
    public partial class ucardPrivileges : WeiXinPage
    {
        protected string openid = "";
        protected int wid = 0;
        protected int sid = 0;//店铺的主键id
        protected int degreeNum = 0;
        protected int uid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            OnlyWeiXinLook();

            openid = MyCommFun.RequestOpenid();
            wid = MyCommFun.RequestInt("wid");
            sid = MyCommFun.RequestInt("sid");
            if (openid == "" || wid == 0 || sid == 0)
            {
                hidStatus.Value = "-1";
                hidErrInfo.Value = "参数错误";
                return;
            }
            bindData();
        }

        private void bindData()
        {
            BLL.wx_ucard_cardinfo cardBll = new BLL.wx_ucard_cardinfo();
            Model.wx_ucard_cardinfo cardinfo = cardBll.GetModelBySid(sid);
            if (cardinfo != null)
            {
                imgTopPic.ImageUrl = cardinfo.privilegesPic;
            }
            BLL.wx_ucard_users userBll = new BLL.wx_ucard_users();
            Model.wx_ucard_users user = userBll.GetStoreUserInfo(openid, sid);
            if (user == null)
            {
                return;
            }
            uid = user.id;
            int degreeNum = 0;
            BLL.wx_ucard_fun.userDegree(sid, MyCommFun.Obj2Int(user.ttScore), "", out degreeNum);

            BLL.wx_ucard_privileges privilegesBLL = new BLL.wx_ucard_privileges();
            IList<Model.wx_ucard_privileges> plist = privilegesBLL.GetModelList(" sid=" + sid + " and ( userDegree ='0' or userDegree like '%," + degreeNum + ",%' ) and beginDate<='" + DateTime.Now + "' and endDate>='" + DateTime.Now + "' ");
            StringBuilder pStr = new StringBuilder();
            if (plist != null && plist.Count > 0)
            {
                Model.wx_ucard_privileges privileges = new Model.wx_ucard_privileges();
                string sn = "";
                for (int i = 0; i < plist.Count; i++)
                {
                    privileges = plist[i];
                    sn = Utils.Number(16, true);
                    if (i == 0)
                    {
                        //第一条数据
                        pStr.Append(" <div id=\"test0-header\" class=\"accordion_headings  header_highlight \">");
                        pStr.Append(" <div class=\"tab  vip \">");
                        pStr.Append(" <span class=\"title\">" + privileges .pName+ "<p>有效期至"+privileges.endDate.Value.ToString("yyyy年MM月dd日")+"</p></span>");
                        pStr.Append(" </div>");
                        pStr.Append(" <div id=\"test0-content\" style=\"display: block; overflow: hidden; opacity: 1;\">");
                        pStr.Append(" <div class=\"accordion_child\">");
                        pStr.Append("  <p class=\"num\" onclick=\"jQ('#test0-content').height(300);document.getElementById('queren0').style.display=''\" id=\"sn0\">" + sn + "</p>");
                        pStr.Append("  <div id=\"queren0\" style=\"display: none\">");
                        pStr.Append("  <p style=\"margin: 10px 0\">");
                        pStr.Append(" <input name=\"\" type=\"text\" class=\"px\" id=\"money0\" value=\"\" placeholder=\"请输入实际消费金额\">");
                        pStr.Append(" </p> <p style=\"margin: 10px 0\">");
                        pStr.Append(" <input name=\"\" type=\"text\" class=\"px\" id=\"bmoney0\" value=\"\" placeholder=\"请再次输入实际消费金额\">");
                        pStr.Append(" </p>  <p style=\"margin: 10px 0 0 0\">");
                        pStr.Append("  <input name=\"\" class=\"px\" id=\"parssword0\" value=\"\" type=\"password\" placeholder=\"请输入管理员密码\">");
                        pStr.Append("  </p> <p style=\"margin: 10px 0\">");
                        pStr.Append("<a id=\"showcard0\" class=\"submit\" href=\"javascript:void(0)\" onclick=\"power(0,'" + sn + "','" + privileges .id+ "')\">确定使用</a>");
                        pStr.Append("   </p> </div>");
                        pStr.Append("  <p class=\"explain_sn\"><span>点击处理</span></p>");
                        pStr.Append(" <b>详情说明</b>");
                        pStr.Append("<ul>"+privileges.usedContent+"</ul></div> </div> </div>");
                        
                    }
                    else
                    {
                        pStr.Append(" <div id=\"test"+i+"-header\" class=\"accordion_headings \">");
                        pStr.Append("  <div class=\"tab  vip \">");
                        pStr.Append(" <span class=\"title\">" + privileges.pName + "<p>有效期至" + privileges.endDate.Value.ToString("yyyy年MM月dd日") + "</p>");
                        pStr.Append(" </span>  </div>");
                        pStr.Append(" <div id=\"test"+i+"-content\" style=\"display: none; overflow: hidden;\">");
                        pStr.Append("  <div class=\"accordion_child\">");
                        pStr.Append("<p class=\"num\" onclick=\"jQ('#test" + i + "-content').height(300);document.getElementById('queren" + i + "').style.display=''\" id=\"sn" + i + "\">" + sn + "</p>");
                        pStr.Append("<div id=\"queren"+i+"\" style=\"display: none\">  <p style=\"margin: 10px 0\">");
                        pStr.Append("  <input name=\"\" type=\"text\" class=\"px\" id=\"money"+i+"\" value=\"\" placeholder=\"请输入实际消费金额\">");
                        pStr.Append("  </p>  <p style=\"margin: 10px 0\">");
                        pStr.Append(" <input name=\"\" type=\"text\" class=\"px\" id=\"bmoney" + i + "\" value=\"\" placeholder=\"请再次输入实际消费金额\">");
                        pStr.Append("  </p> <p style=\"margin: 10px 0 0 0\">");
                        pStr.Append("  <input name=\"\" class=\"px\" id=\"parssword" + i + "\" value=\"\" type=\"password\" placeholder=\"请输入管理员密码\">");
                        pStr.Append("   </p><p style=\"margin: 10px 0\">");
                        pStr.Append(" <a id=\"showcard"+i+"\" class=\"submit\" href=\"javascript:void(0)\" onclick=\"power(" + i + ",'"+sn+"','"+privileges.id+"')\">确定使用</a>");
                        pStr.Append("  </p></div>");
                        pStr.Append(" <p class=\"explain_sn\"><span>点击处理</span></p>");
                        pStr.Append("  <b>详情说明</b>");
                        pStr.Append("  <ul>所有的特权来吧 </ul></div> </div> </div>");
                    }
                }

            }
            litPrivilegeslist.Text = pStr.ToString();

        }

    }
}