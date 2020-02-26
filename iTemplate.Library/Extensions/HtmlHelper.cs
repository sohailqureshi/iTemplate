using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace iTemplate.Library.Extensions
{
  public static class HtmlHelpers
  {
    public static MvcHtmlString DisplayAddressFor<TModel, TProperty>(this HtmlHelper<TModel> self, Expression<Func<TModel, TProperty>> expression, string glyph = "")
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var propertyName = ExpressionHelper.GetExpressionText(expression);
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var value = ModelMetadata.FromLambdaExpression(expression, self.ViewData).Model;

      StringBuilder sb = new StringBuilder();

      sb.Append("<div class='form-group'>");
      sb.AppendFormat("<label class='text-info'>{0}</label>", title);
      sb.AppendFormat("<div class='input-group' data-{0}='{1}'>", propertyName.Replace(".","_"), value);

      if (glyph != null)
      {
        var strGlyph = (glyph != null) ? string.Format(" glyphicon-{0}", glyph.ToLower()) : "";
        sb.AppendFormat("<span class='input-group-addon'><i class='glyphicon{0}'></i></span>", strGlyph);
      }

      sb.Append(string.Format("<label class='form-control'>{0}</label>", value));
      sb.AppendLine("</div>");
      sb.AppendLine("</div>");

      return MvcHtmlString.Create(sb.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="glyph"></param>
    /// <returns></returns>
    public static MvcHtmlString InputAmount<TModel, TProperty>(this HtmlHelper<TModel> self, Expression<Func<TModel, TProperty>> expression, string glyph = "gbp")
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var propertyName = ExpressionHelper.GetExpressionText(expression);
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var format = (!string.IsNullOrEmpty(metadata.EditFormatString)) ? metadata.EditFormatString : "{0:F2}";

      IDictionary<string, object> tbAttributes = new Dictionary<string, object>();
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control price text-right");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", placeHolder);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "title", toolTip);

      var ctrl = self.TextBoxFor(expression, format, tbAttributes);

      return BuildControl(ctrl.ToString(), title, glyph);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="glyph"></param>
    /// <returns></returns>
    public static MvcHtmlString InputInteger<TModel, TProperty>(this HtmlHelper<TModel> self, Expression<Func<TModel, TProperty>> expression, string glyph = "")
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var propertyName = ExpressionHelper.GetExpressionText(expression);
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var format = (!string.IsNullOrEmpty(metadata.EditFormatString)) ? metadata.EditFormatString : "{0:F0}";

      IDictionary<string, object> tbAttributes = new Dictionary<string, object>();
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control price text-right");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", placeHolder);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "title", toolTip);

      var ctrl = self.TextBoxFor(expression, format, tbAttributes);

      return BuildControl(ctrl.ToString(), title, glyph);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public static MvcHtmlString InputDateTimePicker<TModel, TProperty>(this HtmlHelper<TModel> self, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var propertyName = ExpressionHelper.GetExpressionText(expression);
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var format=(!string.IsNullOrEmpty(metadata.EditFormatString))?metadata.EditFormatString:"{0:dd-MMM-yyyy hh:mm tt}";

      string value;
      if (metadata.Model != null)
        value = metadata.Model.ToString();
      else
        value = String.Empty;
          
      IDictionary<string, object> tbAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", placeHolder);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "title", toolTip);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "data-date-format", "DD-MMM-YYYY hh:mm A");

      var ctrl = self.TextBoxFor(expression, format, tbAttributes);

      StringBuilder sb = new StringBuilder();

      sb.Append("<div class='form-group'>");
      sb.AppendFormat("<label class='text-info'>{0}</label>", title);
      sb.AppendFormat("<div class='input-group date' id='dtp{0}'>", propertyName);
      sb.Append(ctrl);
      sb.AppendFormat("<span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span></span>");
      sb.AppendLine("</div>");
      sb.AppendLine(RequiredField(ctrl.ToString()));
      sb.AppendLine("</div>");

      return MvcHtmlString.Create(sb.ToString());
    }   
    
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MvcHtmlString InputText<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
    {
      return InputText(self, expression, null, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="glyph"></param>
    /// <returns></returns>
    public static MvcHtmlString InputText<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string glyph = null)
    {
      return InputText(self, expression, null, glyph);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="glyph"></param>
    /// <returns></returns>
    public static MvcHtmlString InputText<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, string glyph = null)
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var propertyName = ExpressionHelper.GetExpressionText(expression);
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      //var description = ((metadata.Description != null) ? metadata.Description : title);
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var value = ModelMetadata.FromLambdaExpression(expression, self.ViewData).Model;

      IDictionary<string, object> tbAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", placeHolder);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "title", toolTip);
      
      var ctrl = self.TextBoxFor(expression, tbAttributes);

      return BuildControl(ctrl.ToString(), title, glyph);
    }


    public static MvcHtmlString InputDisabled<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string glyph = null)
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var propertyName = ExpressionHelper.GetExpressionText(expression);
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      //var description = ((metadata.Description != null) ? metadata.Description : title);
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var value = ModelMetadata.FromLambdaExpression(expression, self.ViewData).Model;

      IDictionary<string, object> tbAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes("");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", placeHolder);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "disabled", "disabled");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "title", toolTip);

      var ctrl = self.TextBoxFor(expression, tbAttributes);

      return BuildControl(ctrl.ToString(), title, glyph);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="glyph"></param>
    /// <returns></returns>
    public static MvcHtmlString InputPassword<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string glyph = null)
    {
      return InputPassword(self, expression, null, glyph);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="htmlAttributes"></param>
    /// <param name="glyph"></param>
    /// <returns></returns>
    public static MvcHtmlString InputPassword<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, string glyph = null)
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var name = metadata.PropertyName;
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      var description = ((metadata.Description != null) ? metadata.Description : title);
      var value = ModelMetadata.FromLambdaExpression(expression, self.ViewData).Model;

      IDictionary<string, object> tbAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", description);
      var textbox = self.PasswordFor(expression, tbAttributes);

      return BuildControl(textbox.ToString(), title, glyph);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="name"></param>
    /// <param name="title"></param>
    /// <param name="glyph"></param>
    /// <returns></returns>
    private static MvcHtmlString BuildControl(string ctrl, string title, string glyph = null)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("<div class='form-group'>");
      sb.AppendFormat("<label class='text-info'>{0}</label>", title);
      sb.Append("<div class='input-group'>");

      if (glyph != null)
      {
        var strGlyph = (glyph != null) ? string.Format(" glyphicon-{0}", glyph.ToLower()) : "";
        sb.AppendFormat("<span class='input-group-addon'><i class='glyphicon{0}'></i></span>", strGlyph);
      }

      sb.Append(ctrl);
      sb.AppendLine("</div>");
      sb.AppendLine(RequiredField(ctrl));
      sb.AppendLine("</div>");

      return MvcHtmlString.Create(sb.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="name"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    private static MvcHtmlString BuildTextArea(string ctrl, string name, string title)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("<div class='form-group'>");
      sb.AppendFormat("<label class='text-info'>{0}</label>", title);
      sb.Append("<div style='margin-bottom: 15px'>");
      sb.Append(ctrl);
      sb.AppendLine("</div>");
      sb.AppendFormat("<span class='field-validation-valid' data-valmsg-for='{0}' data-valmsg-replace='true'></span>", name);
      sb.AppendLine("</div>");

      return MvcHtmlString.Create(sb.ToString());
    }

    /// <summary>
    /// /
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="htmlAttributes"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static MvcHtmlString InputTextArea<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool disabled = false)
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var name = metadata.PropertyName;
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var value = ModelMetadata.FromLambdaExpression(expression, self.ViewData).Model;

      IDictionary<string, object> tbAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      if (disabled == true) { tbAttributes.Add("disabled", true); }

      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", placeHolder);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "title", toolTip);
      var ctrl = self.TextAreaFor(expression, tbAttributes);

      return BuildTextArea(ctrl.ToString(), name, title);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="text"></param>
    /// <param name="actionName"></param>
    /// <param name="buttonStyle"></param>
    /// <returns></returns>
    public static MvcHtmlString Button(this HtmlHelper htmlHelper, string text, string actionName = "Index", string buttonStyle = "default")
    {
      var controller = htmlHelper.ViewContext.RouteData.Values["controller"];

      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("<a class='btn btn-{0}' href='{1}'>{2}</a>", buttonStyle, "/" + controller + "/" + actionName, text);

      return MvcHtmlString.Create(sb.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="htmlLabelAttributes"></param>
    /// <param name="htmlCheckBoxAttributes"></param>
    /// <returns></returns>
    public static MvcHtmlString InputCheckBox<TModel>(this HtmlHelper<TModel> self, Expression<Func<TModel, bool>> expression, object htmlAttributes = null)
    {
      var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);

      var name = metadata.PropertyName;
      var title = (metadata.DisplayName != null) ? metadata.DisplayName : metadata.PropertyName;
      var placeHolder = ((metadata.Watermark != null) ? metadata.Watermark : title);
      var toolTip = ((metadata.Description != null) ? metadata.Description : placeHolder);
      var value = ModelMetadata.FromLambdaExpression(expression, self.ViewData).Model;

      IDictionary<string, object> tbAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "class", "form-control");
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "placeholder", placeHolder);
      tbAttributes = AddOrUpdateDictionary(tbAttributes, "title", toolTip);
      var ctrl = self.CheckBoxFor(expression, tbAttributes);

      //var checkbox = self.CheckBoxFor(expression, htmlCheckBoxAttributes);
      //var label = self.LabelFor(expression, htmlCheckBoxAttributes);
      //string text = Regex.Match(label.ToString(), "(?<=^|>)[^><]+?(?=<|$)").Value;

      //var labelTag = new TagBuilder("label");
      //labelTag.AddCssClass("text-info");
      //labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
      //labelTag.InnerHtml = checkbox.ToString() + text;

      //return new MvcHtmlString("<div style='margin-bottom: 15px' class='input-group'>" + labelTag.ToString() + "</div>");

      StringBuilder sb = new StringBuilder();
      sb.Append("<div class='form-group'>");
      sb.AppendLine("<div style='margin-bottom: 15px' class='input-group'>");
      sb.AppendLine("<label class='text-info'>");
      sb.Append(ctrl);
      sb.AppendFormat(" {0}</label>", title);
      sb.AppendLine("</div>");
      sb.AppendLine(RequiredField(ctrl.ToString()));
      sb.AppendLine("</div>");

      return MvcHtmlString.Create(sb.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="IsRequired"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string RequiredField(string ctrl)
    {
      StringBuilder sb = new StringBuilder();
      int isNamed = ctrl.IndexOf("name=") + 5;
      int isRequired = ctrl.IndexOf("data-val-required");

      if (isNamed > 5 && isRequired > 0)
      {
        string name = ctrl.Substring(isNamed);
        name = name.Substring(0, name.IndexOf(" "));

        sb.AppendFormat("<span class='field-validation-valid' data-valmsg-for={0} data-valmsg-replace='true'></span>", name);
      }

      return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    private static IDictionary<string, object> AddOrUpdateDictionary(IDictionary<string, object> dic, string key, string newValue)
    {
      object val;
      if (dic.TryGetValue(key, out val))
      {
        // yay, value exists!
        dic[key] = val + " " + newValue.Trim();
      }
      else
      {
        // darn, lets add the value
        dic.Add(key, newValue);
      }

      return dic;
    }
  }
 }