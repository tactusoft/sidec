<%@ Page Title="" Language="C#" MasterPageFile="~/Authentic.Master" AutoEventWireup="true" CodeBehind="rptActosAdmin.aspx.cs" Inherits="SIDec.rptActosAdmin" MaintainScrollPositionOnPostback="true" ViewStateMode="Enabled" EnableEventValidation="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  <div class="divContent"> 
    <div class="divSPHeaderTitle mb20">Actos administrativos</div>
    <br />   
    <fieldset class="full">
    <legend class="full">Parametros</legend>
    <label class="lbl2 lblDis w80">Actos Entre</label>
      <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFecIni" PopupButtonID="txtFecIni" Format="yyyy-MM-dd"/> 
      <asp:TextBox runat="server" ID="txtFecIni" CssClass="txt2 w90 " TextMode="Date" TabIndex="3"></asp:TextBox> 
   
      <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFecFin" PopupButtonID="txtFecFin" Format="yyyy-MM-dd"/> 
      <asp:TextBox runat="server" ID="txtFecFin" CssClass="txt2 w90 " TextMode="Date" TabIndex="3"></asp:TextBox> 


    <label class="lbl2 lblDis w80">Tipo Acto</label>
      <asp:DropDownList runat="server" ID="ddlbTipoActo" CssClass="txt2 w130" AppendDataBoundItems="true" TabIndex="5"> 
        <asp:ListItem Value="">Todos los Tipos</asp:ListItem>
      </asp:DropDownList>   

    <label class="lbl2 lblDis w100">Causal Acto</label>
      <asp:DropDownList runat="server" ID="ddlbCausalActo" CssClass="txt2 w250" AppendDataBoundItems="true" TabIndex="5"> 
        <asp:ListItem Value="">Todas las Causales</asp:ListItem>
      </asp:DropDownList> 

    <label class="lbl2 lblDis w120">Predio Declarado</label>
      <ajaxToolkit:TextBoxWatermarkExtender runat="server" ID="tbw" TargetControlID="txtPredioDec" WatermarkText="Todos los Predios" />
      <asp:TextBox runat="server" ID="txtPredioDec" CssClass="txt2 w120" TabIndex="3" ></asp:TextBox> 
    <br />
    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn-sm btn-primary w100 mr5" CausesValidation="false" OnClick="btnBuscar_Click" />
  
    <br /><br />
  
  </fieldset>

    <br /><br />

    <asp:HiddenField ID="hfGVReporteSV" runat="server"/> 
    <asp:HiddenField ID="hfGVReporteSH" runat="server"/> 
    <div class="">
      <asp:UpdatePanel runat="server" ID="upReporte" UpdateMode="Conditional">
        <ContentTemplate>
          <asp:Label runat="server" ID="total" CssClass="text-danger"></asp:Label>
            <div class="divSPGV">              
              <asp:GridView id="gvReporte" CssClass="gv" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                EmptyDataText="No hay Proyectos" AllowPaging="true" PageSize="500" OnPageIndexChanging="gvReporte_PageIndexChanging"> 
                <Columns>
                  <asp:BoundField DataField="Cod. Predio" HeaderText="Código Predio Declarado"/>
                  <asp:BoundField DataField="CHIP" HeaderText="CHIP"/> 
                  <asp:BoundField DataField="Tipo Acto" HeaderText="Tipo Acto"/>      
                  <asp:BoundField DataField="Num. Acto" HeaderText="Número Acto"/>
                  <asp:BoundField DataField="Fecha Acto" HeaderText="Fecha Aacto" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}"/> 
                  <asp:BoundField DataField="Estado Predio" HeaderText="Estado Predio Declarado"/>
                  <asp:BoundField DataField="Causal Acto" HeaderText="Causal"/>  
                  <asp:BoundField DataField="Fecha Ejecutoria" HeaderText="Fecha Ejecutoria Acto" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}"/>
                </Columns>
                <SelectedRowStyle CssClass="gvItemSelected"/>
                <HeaderStyle CssClass="gvHeader"/>                            
                  <RowStyle CssClass="gvItem"/>
                  <PagerStyle CssClass="gvPager"/>                           
              </asp:GridView> 


            </div>
        </ContentTemplate>
        <Triggers>
   	        <asp:AsyncPostBackTrigger ControlID="gvReporte" EventName="PageIndexChanging"/>     
   		    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"/> 
        </Triggers>
      </asp:UpdatePanel>
    </div>

    <%--<asp:Button runat="server" ID="btnExportar" Text="Guardar Reporte" OnClick="btnExportar_Click"/>--%>
      <asp:Button runat="server" ID="btnExportar" Text="Guardar Reporte" CssClass="btn btn-sm btn-success w120" OnClick="btnExportar_Click"/>
  </div>


  <%--********************************************************************************************************************************************************************************--%>
  <%--*****************************************************************       SECTION DE SCRIPTS       ******************************************************************--%>
  <%--********************************************************************************************************************************************************************************--%>
  <script>
    function pageLoad()
    {
      $('#<%=gvReporte.ClientID%>').gridviewScroll({
        width: 1198,
        height:200,
        railcolor: gvValores("railcolor"),
        barcolor: gvValores("barcolor"),
        barhovercolor: gvValores("barhovercolor"),
        bgcolor: gvValores("bgcolor"),
        varrowtopimg: gvValores("varrowtopimg"),
        varrowbottomimg: gvValores("varrowbottomimg"),
        harrowleftimg: gvValores("harrowleftimg"),
        harrowrightimg: gvValores("harrowrightimg"),
        freezesize: 2,
        arrowsize: 16,
        headerrowcount: 1,
        railsize: 16,
        barsize: 12,

        startVertical: $("#<%=hfGVReporteSV.ClientID%>").val(), 
        startHorizontal: $("#<%=hfGVReporteSH.ClientID%>").val(), 
        onScrollVertical: function (delta) 
        { 
          $("#<%=hfGVReporteSV.ClientID%>").val(delta); 
        }, 
        onScrollHorizontal: function (delta) 
        { 
          $("#<%=hfGVReporteSH.ClientID%>").val(delta); 
        }
      }); 
    }
  </script>

</asp:Content>