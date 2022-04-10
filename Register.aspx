<%@ Page Title="Register" Async="true" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Login_demo.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form>
    <br />
    <br />
    <div align="center" class="container jumbotron bg-light" style="width:35%">
  <!-- Email input -->
  <div class="form-outline mb-4">
      <br />
      <h1>
      <label class="form-label">Register new user:</label></h1>
&nbsp;<asp:TextBox ID="loginTb" runat="server" CssClass="form-control"></asp:TextBox>
    <label class="form-label" for="form2Example1">Email address</label>
  </div>

  <!-- Password input -->
  <div class="form-outline mb-4">
      <asp:TextBox ID="passTb" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
    <label class="form-label" for="form2Example2">Password</label>
  </div>

  <!-- 2 column grid layout for inline styling -->
  <div class="row mb-4">
    <div class="col d-flex justify-content-center">
         <asp:Button ID="Button1" runat="server" Text="Register" class="btn btn-primary" OnClick="Button1_Click" />
    </div>
  </div>
        <br />
        </div>
</form>
</asp:Content>
