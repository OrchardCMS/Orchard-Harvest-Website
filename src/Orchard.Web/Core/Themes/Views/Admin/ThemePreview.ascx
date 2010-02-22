﻿<%@ Control Language="C#" Inherits="Orchard.Mvc.ViewUserControl<PreviewViewModel>" %>
<%@ Import Namespace="Orchard.Core.Themes.ViewModels"%>
<style type="text/css">
#themepreview, #themepreview fieldset, #themepreview input, #themepreview select 
{
    margin: 0; padding: 0; border:none; font-size: 1em; width:auto; color: #000;
    font-family:Frutiger,"Frutiger Linotype",Univers,Calibri,"Gill Sans","Gill Sans MT","Myriad Pro",Myriad,"DejaVu Sans Condensed","Liberation Sans","Nimbus Sans L",Tahoma,Geneva,"Helvetica Neue",Helvetica,Arial,sans-serif;
}
#themepreview { background: #000; font-size:15px; padding:5px; }
#themepreview span { color: #ccc; padding-right:5px;  }
#themepreview fieldset {  margin: 0; padding: 3px; }
#themepreview button { font-size: 13px; padding:0 3px; margin-left:10px; }
#themepreview button.preview { margin-left:0; }
html.dyn #themepreview button.preview { display:none; }
#themepreview button.cancel { float:right; }
</style>
<div id="themepreview">
<% using(Html.BeginFormAntiForgeryPost(Url.Action("Preview", new{Controller="Admin", Area="Themes"}), FormMethod.Post, new { @class = "inline" })) { %>
    <fieldset>    
        <span><%=T("You are previewing: ")%></span>
        <%=Html.DropDownList("ThemeName", Model.Themes, new {onChange = "this.form.submit();"})%>
        <button type="submit" class="preview" title="<%=_Encoded("Preview")%>" name="submit.Preview" value="<%=_Encoded("Preview")%>"><%=_Encoded("Preview")%></button>
        <button type="submit" title="<%=_Encoded("Apply")%>" name="submit.Apply" value="<%=_Encoded("Apply")%>"><%=_Encoded("Apply this theme") %></button>
        <button type="submit" class="cancel" title="<%=_Encoded("Cancel")%>" name="submit.Cancel" value="<%=_Encoded("Cancel")%>"><%=_Encoded("Cancel")%></button>
        <%=Html.Hidden("ReturnUrl", Context.Request.Url)%>
    </fieldset>
<% } %>
</div>