Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Win.SystemModule
Imports DevExpress.ExpressApp.Actions

Namespace WinSolution.Module.Win
	Public Class UpdateNewActionTreeViewController
		Inherits ViewController
		Private Const DefaultReason As String = "MyKey"
		Private currentObjectType As Type = Nothing
		Public Sub New()
			TargetObjectType = GetType(Category)
			TargetViewNesting = Nesting.Root
		End Sub
		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			If TypeOf View Is ListView Then
				AddHandler ObjectSpace.Reloaded, AddressOf ObjectSpace_Reloaded
				AddHandler View.CurrentObjectChanged, AddressOf ListView_CurrentObjectChangedEventHandler
				AddHandler (CType(View, ListView)).CreateCustomCurrentObjectDetailView, AddressOf ListView_CreateCustomCurrentObjectDetailView
			ElseIf TypeOf View Is DetailView Then
				UpdateActionInDetailView()
				AddHandler View.CurrentObjectChanged, AddressOf DetailView_CurrentObjectChangedEventHandler
			End If
		End Sub
		Protected Overrides Sub OnDeactivated()
			MyBase.OnDeactivated()
			currentObjectType = Nothing
			If TypeOf View Is ListView Then
				RemoveHandler View.ObjectSpace.Reloaded, AddressOf ObjectSpace_Reloaded
				RemoveHandler View.CurrentObjectChanged, AddressOf ListView_CurrentObjectChangedEventHandler
				RemoveHandler (CType(View, ListView)).CreateCustomCurrentObjectDetailView, AddressOf ListView_CreateCustomCurrentObjectDetailView
			End If
			If TypeOf View Is DetailView Then
				RemoveHandler View.CurrentObjectChanged, AddressOf DetailView_CurrentObjectChangedEventHandler
			End If
		End Sub
		Private Sub ObjectSpace_Reloaded(ByVal sender As Object, ByVal e As EventArgs)
			UpdateActionInListView()
		End Sub
		Private Sub ListView_CurrentObjectChangedEventHandler(ByVal sender As Object, ByVal e As EventArgs)
			If View.CurrentObject IsNot Nothing Then
				If Not(currentObjectType Is View.CurrentObject.GetType()) Then
					currentObjectType = View.CurrentObject.GetType()
					UpdateActionInListView()
				End If
			Else
				currentObjectType = Nothing
				UpdateActionInListView()
			End If
		End Sub
		Private Sub DetailView_CurrentObjectChangedEventHandler(ByVal sender As Object, ByVal e As EventArgs)
			UpdateActionInDetailView()
		End Sub
		Private Sub UpdateActionInDetailView()
			If View.CurrentObject Is Nothing Then
				Return
			End If
			Dim action As DevExpress.ExpressApp.Actions.SingleChoiceAction = Frame.GetController(Of WinNewObjectViewController)().NewObjectAction
			action.BeginUpdate()
			For Each item As ChoiceActionItem In action.Items
				item.Active.SetItemValue(DefaultReason, View.CurrentObject.GetType() Is CType(item.Data, Type))
			Next item
			action.EndUpdate()
		End Sub
		Private Sub UpdateActionInListView()
			Dim action As DevExpress.ExpressApp.Actions.SingleChoiceAction = Frame.GetController(Of WinNewObjectViewController)().NewObjectAction
			action.BeginUpdate()
			For Each item As ChoiceActionItem In action.Items
				Dim itemType As Type = CType(item.Data, Type)

				item.Enabled.RemoveItem(DefaultReason)
				If (itemType Is GetType(Level1) OrElse (Not GetType(Category).IsAssignableFrom(itemType))) AndAlso currentObjectType Is Nothing Then
					Continue For
				End If
				If itemType Is GetType(Level2) AndAlso currentObjectType Is GetType(Level1) Then
					Continue For
				End If
				If itemType Is GetType(Level3) AndAlso currentObjectType Is GetType(Level2) Then
					Continue For
				End If
				item.Enabled.SetItemValue(DefaultReason, False)

				If (Not GetType(Category).IsAssignableFrom(itemType)) Then
					item.Active.SetItemValue(DefaultReason, False)
				End If
			Next item
			action.EndUpdate()
		End Sub
		Private Sub ListView_CreateCustomCurrentObjectDetailView(ByVal sender As Object, ByVal e As CreateCustomCurrentObjectDetailViewEventArgs)
			If e.ListViewCurrentObject IsNot Nothing Then
				e.DetailViewId = Application.FindDetailViewId(e.ListViewCurrentObject.GetType())
			Else
				e.DetailViewId = e.CurrentDetailView.Id
			End If
		End Sub
	End Class
End Namespace