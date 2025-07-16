<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tooltip.ascx.cs" Inherits="SIDec.UserControls.Tooltip" %>
<span class="uc-tooltip-container" id="main" runat="server">
            <i class="fas fa-exclamation-circle"></i>
    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
    <span class="uc-tooltip-open">
        <span id="sToolTip" runat="server" class="uc-tooltip-message">
            <asp:Label ID="lblToolTip" runat="server" ></asp:Label>
        </span>
    </span>
</span>