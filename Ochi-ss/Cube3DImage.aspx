<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Ochi-ss.3DImage.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Cube3DImage.aspx.cs" Inherits="Ochi_ss.Cube3DImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main table-responsive" style="margin: 0 auto; text-align: center;">
      <form id="form1" runat="server" style="display: inline-block; text-align: left;">
        <!-- 寸法テーブル -->
        <table class="table table-bordered" style="margin: 10px auto; width: 40%;">
            <thead>
                <tr>
                    <th>厚み (T)</th>
                    <th>長さ (A)</th>
                    <th>巾 (B)</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                      <asp:TextBox ID="dimT" runat="server"></asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="dimA" runat="server"></asp:TextBox>
                    </td>
                    <td>
                      <asp:TextBox ID="dimB" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- 3D Canvas -->
            <div id="canvas-container" style="width: 800px; height: 500px; margin: 0 auto; border: 1px solid #ccc; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);"></div>
        </form>  
        
    </div>
</asp:Content>
