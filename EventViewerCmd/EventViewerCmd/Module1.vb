Imports System.Diagnostics
Imports System.Diagnostics.Eventing.Reader
Imports System.IO

' Parametre 1 : Application Name
' Parametre 2 : Message
' Parametre 3 : Eventlog Entry Type
'
' EventLogEntryType : EventLogEntryType.Information     = 4
'                     EventLogEntryType.Error           = 1
'                     EventLogEntryType.Warning         = 2
'                     EventLogEntryType.SuccessAudit    = 8
'                     EventLogEntryType.FailureAudit    = 16

Module Module1
    Private eventIDFilePath As String = "c:\temp\eventID.txt"

    Sub Main()
        Try
            Dim args As String() = Environment.GetCommandLineArgs()
            If args.Length <> 4 Then
                ' Console.WriteLine("2 parametre girilmeli.")
                MsgBox("3 parametre girilmeli.")
                Return
            End If

            Dim log As String = "Application"
            'Dim source As String = "MyVBApp"
            'Dim source As String = String.Join(" ", args, 1, args.Length - 1)

            Dim prgName As String = args(0)
            Dim source As String = args(1)
            Dim eventMessage As String = args(2)
            Dim eventType As String = args(3)
            Dim eventID As Integer = GetNextEventID() ' Eventid min 10000 (onbin) ile baslamali. Sistem ile cakismasin
            ' Dim entryType As EventLogEntryType

            eventType = "SuccessAudit"

            If Not EventLog.SourceExists(source) Then
                EventLog.CreateEventSource(source, log)
            End If

            If eventType = "information" Then EventLog.WriteEntry(source, eventMessage, EventLogEntryType.Information, eventID)
            If eventType = "Error" Then EventLog.WriteEntry(source, eventMessage, EventLogEntryType.Error, eventID)
            If eventType = "warning" Then EventLog.WriteEntry(source, eventMessage, EventLogEntryType.Warning, eventID)
            If eventType = "SuccessAudit" Then EventLog.WriteEntry(source, eventMessage, EventLogEntryType.SuccessAudit, eventID)
            If eventType = "FailureAudit" Then EventLog.WriteEntry(source, eventMessage, EventLogEntryType.FailureAudit, eventID)

            ' EventLog.WriteEntry(source, eventMessage, EventLogEntryType.Error, eventID)
            'EventLog.WriteEntry(source, eventMessage, entryType, eventID)
            ' EventLog.WriteEntry(source, eventMessage, entryType, eventID)
            Console.WriteLine("Mesaj Event Viewer'a yazıldı: " & eventMessage)
        Catch ex As Exception
            Console.WriteLine("Bir hata oluştu: " & ex.Message)
        Finally
            Console.WriteLine("Uygulama sonlandı.")
        End Try

        MsgBox("uygulama bitti")
    End Sub

    Private Function GetNextEventID() As Integer
        Dim nextID As Integer = 10000 ' Başlangıç ID değeri

        If File.Exists(eventIDFilePath) Then
            Dim idStr As String = File.ReadAllText(eventIDFilePath)
            If Integer.TryParse(idStr, nextID) Then
                nextID += 1
            End If
        End If

        File.WriteAllText(eventIDFilePath, nextID.ToString())
        Return nextID
    End Function

End Module


