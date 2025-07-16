<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListBox.ascx.cs" Inherits="SIDec.UserControls.ListBox" %>
<%@ Register Src="~/UserControls/Tooltip.ascx" TagName="Tooltip" TagPrefix="uc" %>

<asp:UpdatePanel ID="upListBox" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hddId" runat="server" Value="UserControlListBox" />
        <div class="row row-cols-12 print_nobreak">
            <div class="form-group-sm col-12">
                <asp:Label class="lblBasic" id="lblBoxTest" runat="server"></asp:Label>
                   <asp:RequiredFieldValidator ID="rfv_BoxTest" runat="server" SetFocusOnError="true" CssClass="invalid-feedback" Display="Dynamic"
                       ControlToValidate="lstBoxTest" Enabled="false">
                            <uc:ToolTip width="130px" ToolTip="Dato requerido" runat="server"/>
                   </asp:RequiredFieldValidator><br />
                <asp:ListBox runat="server" ID="lstBoxTest" SelectionMode="Multiple" CssClass="form-control form-control-xs">
                </asp:ListBox>
                <asp:TextBox ID="txtBoxTest" runat="server" Visible="false" CssClass="form-control form-control-xs" ></asp:TextBox>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    $(document).ready(function () {
        $(<%=lstBoxTest.ClientID%>).SumoSelect();
        });
</script>
