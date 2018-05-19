Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace WinSolution.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal session As Session, ByVal currentDBVersion As Version)
			MyBase.New(session, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			If Session.FindObject(Of Level1)(Nothing) Is Nothing Then
				Dim l1 As New Level1(Session, "1")
				Dim l2 As New Level2(Session, "2")
				Dim l3 As New Level3(Session, "3")
				l1.Level2s.Add(l2)
				l2.Level3s.Add(l3)
				l1.Save()
				l2.Save()
				l3.Save()
				l1 = New Level1(Session, "1a")
				l2 = New Level2(Session, "2a")
				l3 = New Level3(Session, "3a")
				l1.Level2s.Add(l2)
				l2.Level3s.Add(l3)
				l1.Save()
				l2.Save()
				l3.Save()
			End If
		End Sub
	End Class
End Namespace
