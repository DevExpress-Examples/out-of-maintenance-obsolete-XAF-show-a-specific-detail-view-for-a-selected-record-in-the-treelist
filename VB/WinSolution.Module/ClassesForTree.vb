Imports Microsoft.VisualBasic
Imports DevExpress.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports System.ComponentModel
Imports DevExpress.Persistent.Base.General

Namespace WinSolution.Module
	<NavigationItem> _
	Public MustInherit Class Category
		Inherits BaseObject
		Implements ITreeNode
		Private name_Renamed As String
		Protected MustOverride ReadOnly Property Parent() As ITreeNode
		Protected MustOverride ReadOnly Property Children() As IBindingList
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Property Name() As String
			Get
				Return name_Renamed
			End Get
			Set(ByVal value As String)
				name_Renamed = value
				SetPropertyValue("Name", name_Renamed, value)
			End Set
		End Property
		#Region "ITreeNode"
		Private ReadOnly Property ITreeNode_Children() As IBindingList Implements ITreeNode.Children
			Get
				Return Children
			End Get
		End Property
		Private ReadOnly Property ITreeNode_Name() As String Implements ITreeNode.Name
			Get
				Return Name
			End Get
		End Property
		Private ReadOnly Property ITreeNode_Parent() As ITreeNode Implements ITreeNode.Parent
			Get
				Return Parent
			End Get
		End Property
		#End Region
	End Class
	Public Class Level1
		Inherits Category
		Protected Overrides ReadOnly Property Parent() As ITreeNode
			Get
				Return Nothing
			End Get
		End Property
		Protected Overrides ReadOnly Property Children() As IBindingList
			Get
				Return Level2s
			End Get
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Sub New(ByVal session As Session, ByVal name As String)
			MyBase.New(session)
			Me.Name = name
		End Sub
		<Association("Level1-Level2s"), Aggregated> _
		Public ReadOnly Property Level2s() As XPCollection(Of Level2)
			Get
				Return GetCollection(Of Level2)("Level2s")
			End Get
		End Property
	End Class
	Public Class Level2
		Inherits Category
		Private _level1 As Level1
		Protected Overrides ReadOnly Property Parent() As ITreeNode
			Get
				Return _level1
			End Get
		End Property
		Protected Overrides ReadOnly Property Children() As IBindingList
			Get
				Return Level3s
			End Get
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Sub New(ByVal session As Session, ByVal name As String)
			MyBase.New(session)
			Me.Name = name
		End Sub
		<Association("Level1-Level2s")> _
		Public Property Level1() As Level1
			Get
				Return _level1
			End Get
			Set(ByVal value As Level1)
				SetPropertyValue("Level1", _level1, value)
			End Set
		End Property
		<Association("Level2-Level3s"), Aggregated> _
		Public ReadOnly Property Level3s() As XPCollection(Of Level3)
			Get
				Return GetCollection(Of Level3)("Level3s")
			End Get
		End Property
	End Class
	Public Class Level3
		Inherits Category
		Private _level2 As Level2
		Protected Overrides ReadOnly Property Parent() As ITreeNode
			Get
				Return _level2
			End Get
		End Property
		Protected Overrides ReadOnly Property Children() As IBindingList
			Get
				Return New BindingList(Of Object)()
			End Get
		End Property
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Public Sub New(ByVal session As Session, ByVal name As String)
			MyBase.New(session)
			Me.Name = name
		End Sub
		<Association("Level2-Level3s")> _
		Public Property Level2() As Level2
			Get
				Return _level2
			End Get
			Set(ByVal value As Level2)
				SetPropertyValue("Level2", _level2, value)
			End Set
		End Property
	End Class
End Namespace