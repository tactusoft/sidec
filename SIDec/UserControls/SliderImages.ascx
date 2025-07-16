<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SliderImages.ascx.cs" Inherits="SIDec.UserControls.SliderImages" %>
<link rel="stylesheet" href="styles/jquery/jcarousel.basic.css" />
<script type="text/javascript" src="styles/jquery/jquery.jcarousel.min.js"></script>

<div id="mycarousel" runat="server" class="jcarousel jcarousel-skin-tango" style="width: 200px; height: 200px">
    <asp:Repeater ID="rptCarousel" runat="server">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <a href="<%# "Handler1.ashx?fileFoto="+Eval("Path") %>" title="" target="_blank">
                    <asp:Image ID="imgItem" runat="server" ImageUrl='<%#"../Handler1.ashx?fileFoto="+Eval("Path")%>' Width="200" Height="200" />
                </a>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>

<a class="jcarousel-control-prev" href="#">‹</a>
<a class="jcarousel-control-next" href="#">›</a>


