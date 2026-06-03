Public Class Form1

    ' =========================================
    ' LIGHT STATES
    ' =========================================

    Public Enum LightState
        Red
        Yellow
        Green
    End Enum

    ' =========================================
    ' SYSTEM VARIABLES
    ' =========================================
    Private pedestrianLabels As New List(Of Label)
    Private trafficGroups As New List(Of TrafficGroup)

    Private leftSignals As New List(Of Panel)

    Private currentGroup As Integer = 0

    Private currentCountdown As Integer = 10

    Private currentState As LightState =
        LightState.Green

    Private Const GREEN_DURATION As Integer = 10
    Private Const YELLOW_DURATION As Integer = 3

    ' =========================================
    ' LEFT TURN
    ' =========================================

    Private isLeftTurnPhase As Boolean = True

    Private currentLeftCountdown As Integer = 5

    Private Const LEFT_DURATION As Integer = 5

    Private pedestrianPanels As New List(Of Panel)

    Private pedestrianRequests(3) As Boolean

    Private pedestrianPhase As Boolean = False

    Private currentPedestrianGroup As Integer = -1

    Private pedestrianCountdown As Integer = 10

    Private Const PEDESTRIAN_DURATION As Integer = 10

    ' =========================================
    ' FORM LOAD
    ' =========================================

    Private Sub Form1_Load(sender As Object,
                           e As EventArgs) _
                           Handles MyBase.Load

        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.UserPaint Or
                    ControlStyles.DoubleBuffer, True)

        ' =====================================
        ' GROUP 1
        ' =====================================

        Dim g1 As New TrafficGroup()

        g1.Lights.Add(New TrafficLight(pnlLight1, lblTimer1, LightState.Red))
        g1.Lights.Add(New TrafficLight(pnlLight2, lblTimer2, LightState.Red))
        g1.Lights.Add(New TrafficLight(pnlLight3, lblTimer3, LightState.Red))
        g1.Lights.Add(New TrafficLight(pnlLight4, lblTimer4, LightState.Red))

        ' =====================================
        ' GROUP 2
        ' =====================================

        Dim g2 As New TrafficGroup()

        g2.Lights.Add(New TrafficLight(pnlLight5, lblTimer5, LightState.Red))
        g2.Lights.Add(New TrafficLight(pnlLight6, lblTimer6, LightState.Red))
        g2.Lights.Add(New TrafficLight(pnlLight7, lblTimer7, LightState.Red))
        g2.Lights.Add(New TrafficLight(pnlLight8, lblTimer8, LightState.Red))

        ' =====================================
        ' GROUP 3
        ' =====================================

        Dim g3 As New TrafficGroup()

        g3.Lights.Add(New TrafficLight(pnlLight9, lblTimer9, LightState.Red))
        g3.Lights.Add(New TrafficLight(pnlLight10, lblTimer10, LightState.Red))
        g3.Lights.Add(New TrafficLight(pnlLight11, lblTimer11, LightState.Red))
        g3.Lights.Add(New TrafficLight(pnlLight12, lblTimer12, LightState.Red))

        ' =====================================
        ' GROUP 4
        ' =====================================

        Dim g4 As New TrafficGroup()

        g4.Lights.Add(New TrafficLight(pnlLight13, lblTimer13, LightState.Red))
        g4.Lights.Add(New TrafficLight(pnlLight14, lblTimer14, LightState.Red))
        g4.Lights.Add(New TrafficLight(pnlLight15, lblTimer15, LightState.Red))
        g4.Lights.Add(New TrafficLight(pnlLight16, lblTimer16, LightState.Red))

        trafficGroups.Add(g1)
        trafficGroups.Add(g2)
        trafficGroups.Add(g3)
        trafficGroups.Add(g4)

        ' =====================================
        ' LEFT SIGNALS
        ' =====================================

        leftSignals.Add(pnlLeft1)
        leftSignals.Add(pnlLeft2)
        leftSignals.Add(pnlLeft3)
        leftSignals.Add(pnlLeft4)

        leftSignals.Add(pnlLeft5)
        leftSignals.Add(pnlLeft6)
        leftSignals.Add(pnlLeft7)
        leftSignals.Add(pnlLeft8)

        leftSignals.Add(pnlLeft9)
        leftSignals.Add(pnlLeft10)
        leftSignals.Add(pnlLeft11)
        leftSignals.Add(pnlLeft12)

        leftSignals.Add(pnlLeft13)
        leftSignals.Add(pnlLeft14)
        leftSignals.Add(pnlLeft15)
        leftSignals.Add(pnlLeft16)

        ' =====================================
        ' PEDESTRIAN PANELS
        ' =====================================

        pedestrianPanels.Add(pnlPed1)
        pedestrianPanels.Add(pnlPed2)
        pedestrianPanels.Add(pnlPed3)
        pedestrianPanels.Add(pnlPed4)

        ' =====================================
        ' PEDESTRIAN LABELS
        ' =====================================

        pedestrianLabels.Add(lblPed1)
        pedestrianLabels.Add(lblPed2)
        pedestrianLabels.Add(lblPed3)
        pedestrianLabels.Add(lblPed4)

        For Each pnl In pedestrianPanels

            AddHandler pnl.Paint,
        AddressOf DrawPedestrianSignal

            pnl.Invalidate()
            For Each lbl In pedestrianLabels

                lbl.Text = "STOP"

                lbl.ForeColor = Color.Red

            Next

        Next

        ' =====================================
        ' DRAW LEFT ARROWS
        ' =====================================

        For Each pnl In leftSignals

            AddHandler pnl.Paint,
                AddressOf DrawLeftArrow

            pnl.Visible = False

        Next

        ' SHOW FIRST GROUP
        For i As Integer = 0 To 3

            leftSignals(i).Visible = True

        Next

        UpdateLabels()

        systemTimer.Interval = 1000

        systemTimer.Start()

    End Sub
    Private Sub DrawPedestrianSignal(sender As Object,
                                 e As PaintEventArgs)

        Dim pnl As Panel =
        CType(sender, Panel)

        Dim index As Integer =
        pedestrianPanels.IndexOf(pnl)

        Dim g As Graphics = e.Graphics

        g.SmoothingMode =
        Drawing2D.SmoothingMode.AntiAlias

        Dim brush As Brush

        If pedestrianPhase AndAlso
       index = currentPedestrianGroup Then

            brush = New SolidBrush(Color.Lime)

        Else

            brush = New SolidBrush(Color.Red)

        End If

        g.FillEllipse(brush, 10, 10, 30, 30)

    End Sub


    ' =========================================
    ' TIMER
    ' =========================================

    Private Sub systemTimer_Tick(sender As Object,
                                 e As EventArgs) _
                                 Handles systemTimer.Tick
        ' =====================================
        ' PEDESTRIAN PHASE
        ' =====================================
        For i As Integer = 0 To pedestrianLabels.Count - 1

            If i = currentPedestrianGroup Then

                pedestrianLabels(i).Text =
            "WALK " &
            pedestrianCountdown.ToString()

                pedestrianLabels(i).ForeColor =
            Color.Lime

            Else

                pedestrianLabels(i).Text = "STOP"

                pedestrianLabels(i).ForeColor =
            Color.Red

            End If

        Next

        If pedestrianPhase Then

            pedestrianCountdown -= 1

            If pedestrianCountdown <= 0 Then

                pedestrianPhase = False

                For Each lbl In pedestrianLabels

                    lbl.Text = "STOP"

                    lbl.ForeColor = Color.Red

                Next

                pedestrianCountdown =
            PEDESTRIAN_DURATION

                currentPedestrianGroup = -1

                trafficGroups(currentGroup).
            SetState(LightState.Green)

            End If

            For Each pnl In pedestrianPanels

                pnl.Invalidate()

            Next

            Return

        End If
        ' =====================================
        ' LEFT TURN PHASE
        ' =====================================

        If isLeftTurnPhase Then

            currentLeftCountdown -= 1

            UpdateLabels()

            If currentLeftCountdown <= 0 Then

                isLeftTurnPhase = False

                ' HIDE LEFT SIGNALS
                For Each pnl In leftSignals

                    pnl.Visible = False

                Next

                trafficGroups(currentGroup).
                    SetState(LightState.Green)

                currentState = LightState.Green

                currentCountdown = GREEN_DURATION

            End If

            Return

        End If

        ' =====================================
        ' NORMAL TRAFFIC
        ' =====================================

        currentCountdown -= 1

        UpdateLabels()

        If currentCountdown <= 0 Then

            ' GREEN -> YELLOW

            If currentState = LightState.Green Then

                trafficGroups(currentGroup).
                    SetState(LightState.Yellow)

                currentState = LightState.Yellow

                currentCountdown = YELLOW_DURATION

            Else
                ' =====================================
                ' PEDESTRIAN REQUEST
                ' =====================================

                If pedestrianRequests(currentGroup) Then

                    pedestrianRequests(currentGroup) = False

                    pedestrianPhase = True

                    currentPedestrianGroup =
        currentGroup

                    pedestrianCountdown =
        PEDESTRIAN_DURATION

                    ' ALL RED
                    For Each grp In trafficGroups

                        grp.SetState(LightState.Red)

                    Next

                    For Each pnl In pedestrianPanels

                        pnl.Invalidate()

                    Next

                    Return

                End If
                ' YELLOW -> NEXT GROUP

                trafficGroups(currentGroup).
                    SetState(LightState.Red)

                currentGroup =
                    (currentGroup + 1) Mod trafficGroups.Count

                ' START LEFT TURN AGAIN
                isLeftTurnPhase = True

                currentLeftCountdown = LEFT_DURATION

                currentState = LightState.Green

                ' HIDE ALL
                For Each pnl In leftSignals

                    pnl.Visible = False

                Next

                ' SHOW CURRENT GROUP SIGNALS
                Dim startIndex As Integer =
                    currentGroup * 4

                For i As Integer = startIndex To startIndex + 3

                    leftSignals(i).Visible = True

                Next

            End If

        End If

    End Sub

    ' =========================================
    ' UPDATE LABELS
    ' =========================================

    Private Sub UpdateLabels()

        For i As Integer = 0 To trafficGroups.Count - 1

            For Each light In trafficGroups(i).Lights

                If i = currentGroup Then

                    ' LEFT TURN
                    If isLeftTurnPhase Then

                        light.CountdownLabel.Text =
                            currentLeftCountdown.ToString()

                        light.CountdownLabel.ForeColor =
                            Color.Cyan

                    Else

                        light.CountdownLabel.Text =
                            currentCountdown.ToString()

                        If currentState = LightState.Green Then

                            light.CountdownLabel.ForeColor =
                                Color.Lime

                        Else

                            light.CountdownLabel.ForeColor =
                                Color.Yellow

                        End If

                    End If

                Else

                    light.CountdownLabel.ForeColor =
                        Color.Red

                End If

            Next

        Next

    End Sub

    ' =========================================
    ' DRAW LEFT ARROW
    ' =========================================

    Private Sub DrawLeftArrow(sender As Object,
                              e As PaintEventArgs)

        Dim g As Graphics = e.Graphics

        g.SmoothingMode =
            Drawing2D.SmoothingMode.AntiAlias

        Dim brush As New SolidBrush(Color.Cyan)

        Dim pts = {
            New Point(20, 5),
            New Point(5, 20),
            New Point(12, 20),
            New Point(12, 35),
            New Point(28, 35),
            New Point(28, 20),
            New Point(35, 20)
        }

        g.FillPolygon(brush, pts)

    End Sub

    ' =========================================
    ' TRAFFIC LIGHT CLASS
    ' =========================================

    Public Class TrafficLight

        Public Property DrawingPanel As Panel

        Public Property CountdownLabel As Label

        Private _currentColor As Form1.LightState

        Public Property CurrentColor As Form1.LightState

            Get
                Return _currentColor
            End Get

            Set(value As Form1.LightState)

                _currentColor = value

                DrawingPanel.Invalidate()

            End Set

        End Property

        Public Sub New(panel As Panel,
                       label As Label,
                       initialState As Form1.LightState)

            Me.DrawingPanel = panel
            Me.CountdownLabel = label
            Me._currentColor = initialState

            AddHandler Me.DrawingPanel.Paint,
                AddressOf DrawLightCircles

        End Sub

        Private Sub DrawLightCircles(sender As Object,
                                     e As PaintEventArgs)

            Dim g As Graphics = e.Graphics

            g.SmoothingMode =
                Drawing2D.SmoothingMode.AntiAlias

            Dim redBrush As Brush =
                If(_currentColor = Form1.LightState.Red,
                   New SolidBrush(Color.Red),
                   New SolidBrush(Color.FromArgb(60, 0, 0)))

            Dim yellowBrush As Brush =
                If(_currentColor = Form1.LightState.Yellow,
                   New SolidBrush(Color.Yellow),
                   New SolidBrush(Color.FromArgb(60, 60, 0)))

            Dim greenBrush As Brush =
                If(_currentColor = Form1.LightState.Green,
                   New SolidBrush(Color.Lime),
                   New SolidBrush(Color.FromArgb(0, 60, 0)))

            If DrawingPanel.Width > DrawingPanel.Height Then

                Dim diameter As Integer =
                    CInt(DrawingPanel.Height * 0.6)

                Dim spacing As Integer =
                    CInt((DrawingPanel.Width -
                    (diameter * 3)) / 4)

                Dim yPos As Integer =
                    CInt((DrawingPanel.Height -
                    diameter) / 2)

                Dim xRed As Integer = spacing

                Dim xYellow As Integer =
                    (spacing * 2) + diameter

                Dim xGreen As Integer =
                    (spacing * 3) + (diameter * 2)

                g.FillEllipse(redBrush,
                              xRed,
                              yPos,
                              diameter,
                              diameter)

                g.FillEllipse(yellowBrush,
                              xYellow,
                              yPos,
                              diameter,
                              diameter)

                g.FillEllipse(greenBrush,
                              xGreen,
                              yPos,
                              diameter,
                              diameter)

            Else

                Dim diameter As Integer =
                    CInt(DrawingPanel.Width * 0.6)

                Dim xPos As Integer =
                    CInt((DrawingPanel.Width -
                    diameter) / 2)

                Dim spacing As Integer =
                    CInt((DrawingPanel.Height -
                    (diameter * 3)) / 4)

                Dim yRed As Integer = spacing

                Dim yYellow As Integer =
                    (spacing * 2) + diameter

                Dim yGreen As Integer =
                    (spacing * 3) + (diameter * 2)

                g.FillEllipse(redBrush,
                              xPos,
                              yRed,
                              diameter,
                              diameter)

                g.FillEllipse(yellowBrush,
                              xPos,
                              yYellow,
                              diameter,
                              diameter)

                g.FillEllipse(greenBrush,
                              xPos,
                              yGreen,
                              diameter,
                              diameter)

            End If

            redBrush.Dispose()
            yellowBrush.Dispose()
            greenBrush.Dispose()

        End Sub

    End Class

    ' =========================================
    ' TRAFFIC GROUP CLASS
    ' =========================================

    Public Class TrafficGroup

        Public Property Lights As New List(Of TrafficLight)

        Public Sub SetState(state As Form1.LightState)

            For Each light In Lights

                light.CurrentColor = state

            Next

        End Sub

    End Class


    Private Sub btnPed1_Click(sender As Object, e As EventArgs) Handles btnPed1.Click
        pedestrianRequests(0) = True
    End Sub

    Private Sub btnPed2_Click(sender As Object, e As EventArgs) Handles btnPed2.Click
        pedestrianRequests(1) = True
    End Sub

    Private Sub btnPed3_Click(sender As Object, e As EventArgs) Handles btnPed3.Click
        pedestrianRequests(2) = True
    End Sub

    Private Sub btnPed4_Click(sender As Object, e As EventArgs) Handles btnPed4.Click
        pedestrianRequests(3) = True
    End Sub


End Class