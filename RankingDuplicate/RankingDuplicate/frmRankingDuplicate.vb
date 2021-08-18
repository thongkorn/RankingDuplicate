' / ----------------------------------------------------------------
' / Developer : Mr.Surapon Yodsanga (Thongkorn Tubtimkrob)
' / eMail : thongkorn@hotmail.com
' / URL: http://www.g2gnet.com (Khon Kaen - Thailand)
' / Facebook: https://www.facebook.com/g2gnet (For Thailand)
' / Facebook: https://www.facebook.com/commonindy (Worldwide)
' / More info: http://www.g2gnet.com/webboard
' /
' / Purpose: How to sort ranking duplicate in DataGridView without DataBase.
' / Microsoft Visual Basic .NET (2010)
' /
' / This is open source code under @CopyLeft by Thongkorn Tubtimkrob.
' / You can modify and/or distribute without to inform the developer.
' / ----------------------------------------------------------------
Public Class frmRankingDuplicate
    Dim dt As DataTable
    Dim MaxRow As Integer = 15

    ' / ----------------------------------------------------------------
    '// Initialize DataGridView @Run Time
    Private Sub InitGrid()
        With dgvData
            .RowHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .MultiSelect = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .Font = New Font("Tahoma", 10)
            ' Autosize Column
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoResizeColumns()
            '// Even-Odd Color
            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            ' Adjust Header Styles
            With .ColumnHeadersDefaultCellStyle
                .BackColor = Color.Navy
                .ForeColor = Color.Black ' Color.White
                .Font = New Font("Tahoma", 10, FontStyle.Bold)
            End With
        End With
    End Sub

    Private Sub frmRankingDuplicate_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
        GC.SuppressFinalize(Me)
        Application.Exit()
    End Sub

    ' / ----------------------------------------------------------------
    Private Sub frmRankingDuplicate_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Call InitGrid()
        Call Ranking()
    End Sub

    ' / ----------------------------------------------------------------
    Private Sub Ranking()
        Dim ds As New DataSet()
        dt = New DataTable
        dt.Columns.Add("Student", GetType(String))
        dt.Columns.Add("Subject A", GetType(Integer))
        dt.Columns.Add("Subject B", GetType(Integer))
        dt.Columns.Add("Subject C", GetType(Integer))
        dt.Columns.Add("Average", GetType(Double))
        dgvData.DataSource = dt
        '//
        Randomize()
        Dim RandomClass As New Random()
        For i As Integer = 0 To MaxRow - 1
            Dim dr As DataRow = dt.NewRow()
            '// RandomClass
            dr(0) = "Student " & i + 1
            dr(1) = RandomClass.Next(0, 100)
            dr(2) = RandomClass.Next(0, 100)
            dr(3) = RandomClass.Next(0, 100)
            Dim SumCol As Double = 0
            For iCol As Byte = 1 To 3 'MaxCol
                SumCol = SumCol + dr(iCol)
            Next
            dr(4) = Format(SumCol / 3, "0.00")
            '//
            dt.Rows.Add(dr)
        Next
        '// Calcualte ranking
        dgvData.DataSource = RankingDuplicate(dt, "Average")
    End Sub

    ' / ----------------------------------------------------------------
    Public Function RankingDuplicate(dt As DataTable, fld As String) As DataTable
        '// Descending (Z --> A)
        Dim rankingDt = (From row In dt.AsEnumerable() Order By row.Field(Of Double)(fld) Descending.CopyToDataTable())
        rankingDt.Columns.Add("Ranking")
        Dim rank As Integer = 1
        Dim count As Integer = 1
        For i As Integer = 0 To rankingDt.Rows.Count - 2
            rankingDt.Rows(i)("Ranking") = rank
            '// If not duplicate value then increment +1.
            If rankingDt.Rows(i)(fld).ToString() <> rankingDt.Rows(i + 1)(fld).ToString() Then
                rank += 1
                rank = count + 1
            End If
            count += 1
        Next
        rankingDt.Rows(rankingDt.Rows.Count - 1)("Ranking") = rank
        Return rankingDt
    End Function

    Private Sub btnExit_Click(sender As System.Object, e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnProcess_Click(sender As System.Object, e As System.EventArgs) Handles btnProcess.Click
        Call Ranking()
    End Sub
End Class
