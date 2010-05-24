Imports DaveSexton.DocProject
Imports DaveSexton.DocProject.Engine

''' <summary>
''' Hooks into the DocProject build process for the project in which it's defined.
''' </summary>
''' <remarks>
''' <para>
''' This class must be registered with the DocProject in the <em>Active Projects</em>
''' tools options page in order for DocProject to instantiate it during a help build.
''' </para>
''' <para>
''' To cancel the build at any time call the <see cref="BuildContext.Cancel" /> 
''' method.  The build process will end after the current step is executed, 
''' unless the step is being executed in the background.  In that case, it may 
''' end immediately.
''' </para>
''' <para>
''' Note: Do not cache instances of the <see cref="BuildContext" /> class.  A new 
''' <see cref="BuildContext" /> is created each time the project is built.
''' </para>
''' </remarks>
Public Class BuildProcess
	Inherits BuildProcessComponent

	Private buildStart, stepStart As DateTime

	''' <summary>
	''' Called before the project's help build starts.
	''' </summary>
	''' <param name="context">Provides information about the build process.</param>
	Public Overrides Sub BuildStarting(ByVal context As BuildContext)
		' Uncomment the following line to break into the debugger: 
		' System.Diagnostics.Debugger.Break()

		buildStart = DateTime.Now
	End Sub

	''' <summary>
	''' Called before a <paramref name="step" /> is executed during a help build.
	''' </summary>
	''' <param name="buildStep"><see cref="IBuildStep" /> implementation to be executed.</param>
	''' <param name="context">Provides information about the build process.</param>
	''' <returns><b>true</b> indicates that the process should continue, otherwise, 
	''' <b>false</b> indicates that the process should skip this step.</returns>
	Public Overrides Function BeforeExecuteStep(ByVal buildStep As IBuildStep, ByVal context As BuildContext) As Boolean
		stepStart = DateTime.Now
		Return True
	End Function

	''' <summary>
	''' Called after a <paramref name="step" /> has been executed during a help build.
	''' </summary>
	''' <param name="buildStep"><see cref="IBuildStep" /> implementation that was executed.</param>
	''' <param name="context">Provides information about the build process.</param>
	Public Overrides Sub AfterExecuteStep(ByVal buildStep As IBuildStep, ByVal context As BuildContext)
		TraceLine()
		TraceLine("Step {0} Time Elapsed: {1}", context.CurrentStepIndex + 1, DateTime.Now - stepStart)
	End Sub

	''' <summary>
	''' Called after the project's help build has finished.
	''' </summary>
	''' <remarks>
	''' The <see cref="BuildContext.Cancel" /> method has no affect at this 
	''' point in the build process.  This method is the final step before the 
	''' build statistics are displayed.
	''' <para>
	''' This method is always invoked if <see cref="BuildStarting" /> is invoked, 
	''' regardless of whether an exception is thrown in any of the other methods, 
	''' <see cref="BuildContext.Cancel" /> has been called, or an exeception has
	''' been thrown by the build engine.
	''' </para>
	''' <para>
	''' To determine whether a help build failed or succeeded, examine the value of the
	''' <see cref="BuildContext.BuildState" /> property.
	''' </para>
	''' </remarks>
	''' <param name="context">Provides information about the build process.</param>
	Public Overrides Sub BuildCompleted(ByVal context As BuildContext)
		TraceLine()
		TraceLine("Total Time Elapsed: {0}", DateTime.Now - buildStart)
	End Sub
End Class
