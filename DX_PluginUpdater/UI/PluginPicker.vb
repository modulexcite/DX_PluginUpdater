﻿Option Strict On
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.CodeRush.Core
Public Class PluginPicker
#Region "Populate"
    Public Sub PopulatePlugins(ByVal SourcePlugins As IEnumerable(Of String))
        Dim PluginItems = (From Plugin In SourcePlugins _
                          Select New ListViewItem(New String() {Plugin}) With {.Tag = Plugin}).ToArray

        lstPlugins.Items.Clear()
        lstPlugins.Items.AddRange(PluginItems)
    End Sub
    Private Sub PopulatePlugins(ByVal SourcePlugins As IEnumerable(Of RemotePluginRef))
        Dim PluginItems = (From Plugin In SourcePlugins _
                          Select New ListViewItem(New String() {Plugin.PluginName, CStr(Plugin.Version)}) With {.Tag = Plugin}).ToArray

        lstPlugins.Items.Clear()
        lstPlugins.Items.AddRange(PluginItems)
    End Sub
#End Region
#Region "Get"
    Public Shared Function PickPlugins(ByVal SourcePlugins As IEnumerable(Of System.String)) As IEnumerable(Of System.String)
        Dim Form As New PluginPicker
        Form.PopulatePlugins(SourcePlugins)
        If Form.ShowDialog() = DialogResult.OK Then
            Return Form.PickedPluginNames()
        Else
            Return New List(Of String)
        End If
    End Function
    Public Shared Function PickPlugins(ByVal SourcePlugins As IEnumerable(Of RemotePluginRef)) As IEnumerable(Of RemotePluginRef)
        Dim Form As New PluginPicker
        Form.PopulatePlugins(SourcePlugins)
        If Form.ShowDialog() = DialogResult.OK Then
            Return Form.PickedPlugins()
        Else
            Return New List(Of RemotePluginRef)
        End If
    End Function
#End Region
#Region "Picked"
    Private Function PickedPlugins() As IEnumerable(Of RemotePluginRef)
        Return From ListViewItem In lstPlugins.CheckedItems.Cast(Of ListViewItem)()
               Select TryCast(ListViewItem.Tag, RemotePluginRef)
    End Function
    Public Function PickedPluginNames() As IEnumerable(Of String)
        Return From ListViewItem In lstPlugins.CheckedItems.Cast(Of ListViewItem)()
               Select TryCast(ListViewItem.Tag, String)
    End Function
#End Region

#Region "UI"
    Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
        For Each Item As ListViewItem In lstPlugins.Items
            Item.Checked = True
        Next
    End Sub

    Private Sub cmdSelectNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectNone.Click
        For Each Item As ListViewItem In lstPlugins.Items
            Item.Checked = False
        Next
    End Sub
#End Region

    Private Function CurrentPluginName() As String
        Return If(lstPlugins.SelectedItems.Count > 0, TryCast(lstPlugins.SelectedItems(0).Tag, String), "")
    End Function

    Private Sub lstPlugins_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPlugins.SelectedIndexChanged
        lnkWiki.Text = "Wiki: " & CurrentPluginName()
        lnkBinaries.Text = "Binaries:" & CurrentPluginName()
        lnkSource.Text = "Source: " & CurrentPluginName()
    End Sub

    Public Const BaseWikiUrl As String = "http://code.google.com/p/dxcorecommunityplugins/wiki/"
    Public Const BaseBinaryUrl As String = "http://www.rorybecker.co.uk/DevExpress/Plugins/Community/"
    Public Const BaseSourceUrl As String = "http://code.google.com/p/dxcorecommunityplugins/source/browse/trunk/"

    Private Sub lnkWiki_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkWiki.LinkClicked
        System.Diagnostics.Process.Start(BaseWikiUrl & CurrentPluginName())
    End Sub

    Private Sub lnkBinaries_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkBinaries.LinkClicked
        System.Diagnostics.Process.Start(BaseBinaryUrl & CurrentPluginName())
    End Sub

    Private Sub lnkSource_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkSource.LinkClicked
        System.Diagnostics.Process.Start(BaseSourceUrl & CurrentPluginName())
    End Sub
End Class