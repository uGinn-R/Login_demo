<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="Login_demo.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row justify-content-center">
        <table class="table table-responsive table-hover">
                                <tr>
        <td>
<asp:ImageButton ID="btnDelete" ImageUrl="~/img/delete_icon.png" runat="server" Width="25px" OnClick="btnDelete_Click" BorderStyle="None"/>
<asp:ImageButton ID="btnBlock" ImageUrl="~/img/block.png" runat="server" Width="25px" OnClick="btnBlock_Click" BorderStyle="None"/>
<asp:ImageButton ID="btnUnblock" ImageUrl="~/img/unblock.png" runat="server" Width="25px" OnClick="btnUnblock_Click" BorderStyle="None"/>

</td>
    </tr>
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false">
        <Columns>
                 <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox ID="ChkHeader" runat="server" AutoPostBack="true" Text="Select All" OnCheckedChanged="ChkHeader_CheckedChanged" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="ChkEmpty" runat="server" OnCheckedChanged="ChkEmpty_CheckedChanged" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="id" HeaderText="ID" ControlStyle-CssClass="m-4" />
            <asp:BoundField DataField="login" HeaderText="Login" />
            <asp:BoundField DataField="password" HeaderText="Password" />
            <asp:BoundField DataField="lastlogin" HeaderText="Last Logged" />
            <asp:BoundField DataField="regDate" HeaderText="Registration Date" />
            <asp:BoundField DataField="status" HeaderText="STATUS" />

        </Columns>
    </asp:GridView>
    </table>
    </div>
</asp:Content>


