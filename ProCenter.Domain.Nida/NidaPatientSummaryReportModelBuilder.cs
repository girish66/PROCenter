﻿namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using AssessmentModule;
    using Common;
    using Common.Report;
    using CommonModule.Lookups;
    using Pillar.Common.Metadata;
    using Pillar.Common.Utility;

    #endregion

    /// <summary>
    /// Builder for NIDA Patient Summary report model types.
    /// </summary>
    public class NidaPatientSummaryReportModelBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the amber report model.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="dastInstance">The dast instance.</param>
        /// <param name="nidaAssessFurtherInstance">The nida assess further instance.</param>
        /// <returns>A <see cref="ReportModel"/>.</returns>
        public static ReportModel GetAmberReportModel ( IResourcesManager resourcesManager, AssessmentInstance dastInstance, AssessmentInstance nidaAssessFurtherInstance )
        {
            var reportModel = CreateSummarySection(resourcesManager, dastInstance, nidaAssessFurtherInstance);
            reportModel.ReportSeverity = ReportSeverity.Low;
            reportModel.ReportStatus = NidaWorkflowPatientSummaryReport.Amber;
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> (
                                                                                                      r =>
                                                                                                      r.BenefitOfDrugAbstinentFeedback ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.FollowUpHeader ),
                                            null,
                                            null,
                                            new ReportItem ( "FollowUpVisit",
                                                             true, 
                                                             new object[] {
                                                                             DateTime.Now.AddMonths ( 6 )
                                                                                     .ToShortDateString () } ) ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> (
                                                                                                      r =>
                                                                                                      r
                                                                                                          .PatientResourceHeader ),
                                            null,
                                            null,
                                            new ReportItem("UsDepartmentHealthAndHumanServices",
                                                             true)));
            reportModel.AddReportItem(new ReportItem(
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string>(r => r.AdditionalNotes)));

            reportModel.IsPatientViewable = true;
            return reportModel;
        }

        /// <summary>
        /// Gets the green report model.
        /// </summary>
        /// <returns>A <see cref="ReportModel"/>.</returns>
        public static ReportModel GetGreenReportModel ()
        {
            var reportModel = new ReportModel
                {
                    Name = ReportNames.NidaPatientSummaryReport,
                    IsCustomizable = true,
                    ReportSeverity = ReportSeverity.Good,
                    ReportStatus = NidaWorkflowPatientSummaryReport.Green
                };
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.PositiveFeedback ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.FollowUpHeader ),
                                            null,
                                            null,
                                            new ReportItem ( "FollowUpVisit",
                                                             true,
                                                             new object[] { DateTime.Now.AddYears(1).ToShortDateString() })));
            reportModel.AddReportItem(new ReportItem(
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string>(r => r.AdditionalNotes)));

            reportModel.IsPatientViewable = true;
            return reportModel;
        }

        /// <summary>
        /// Gets the red report model.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="dastInstance">The dast instance.</param>
        /// <param name="nidaAssessFurtherInstance">The nida assess further instance.</param>
        /// <returns>A <see cref="ReportModel"/>.</returns>
        public static ReportModel GetRedReportModel ( IResourcesManager resourcesManager, AssessmentInstance dastInstance, AssessmentInstance nidaAssessFurtherInstance )
        {
            var reportModel = CreateSummarySection ( resourcesManager, dastInstance, nidaAssessFurtherInstance );
            reportModel.ReportSeverity = ReportSeverity.High;
            reportModel.ReportStatus = NidaWorkflowPatientSummaryReport.High;
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.RecommendCessation ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.ReferralForFurtherAssessment ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.FollowUpHeader ),
                                            null,
                                            null,
                                            new ReportItem ( "AssessReadinessToChange", true ),
                                            new ReportItem ( "ReviewCurrentMedication", true ),
                                            new ReportItem ( "MentalHealthCondition", true ),
                                            new ReportItem ( "PreventiveScreenings", true ),
                                            new ReportItem ( "FollowUpVisit",
                                                             true,
                                                             new object[] {
                                                                             DateTime.Now.AddMonths ( 6 )
                                                                                     .ToShortDateString ()} ) ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.UseTreatmentHistoryHeader ),
                                            null,
                                            null,
                                            new ReportItem ( "ObtainDrugTreatmentHistory", true ),
                                            new ReportItem ( "ObtainTobaccoAlcoholHistory", true )
                                            ) {ItemMetadata = new ItemMetadata ()} );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.SupportGroups ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.UrineDrugScreen ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> (
                                                                                                      r => r.PatientResourceHeader ),
                                            null,
                                            null,
                                            new ReportItem( "UsDepartmentHealthAndHumanServices", true))
                );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.OpioidDependenceOnSiteTreatment ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.OpioidDependenceOffSiteTreatment ),
                                            true ) );
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.AdditionalNotes ) ) );

            reportModel.IsPatientViewable = true;
            return reportModel;
        }

        public static void FillDefaults ( ReportModel reportModel )
        {
            reportModel.RecurseReportItems ( reportItem =>
                {
                    if ( reportItem.Text == null )
                    {
                        reportItem.Update ( reportItem.ShouldShow,
                                            reportItem.FormatParameters.Any () ? 
                                                string.Format ( NidaWorkflowPatientSummaryReport.ResourceManager.GetString ( reportItem.Name ) ?? string.Empty, reportItem.FormatParameters.ToArray () )
                                                : NidaWorkflowPatientSummaryReport.ResourceManager.GetString(reportItem.Name));
                    }
                    return true;
                } );
        }

        #endregion

        #region Methods

        private static ReportModel CreateSummarySection ( IResourcesManager resourcesManager, AssessmentInstance dastInstance, AssessmentInstance nidaAssessFurtherInstance )
        {
            var reportModel = new ReportModel
                {
                    Name = ReportNames.NidaPatientSummaryReport,
                    IsCustomizable = true
                };
            var reportItems = new List<ReportItem> ();
            var nidaAssessFurtherResources = resourcesManager == null
                                                 ? new ResourceManager ( typeof(NidaAssessFurther) )
                                                 : resourcesManager.GetResourceManagerByName ( NidaAssessFurther.AssessmentCodedConcept.Code );

            var otherDrug = string.Empty;
            foreach ( var itemDefinition in NidaAssessFurther.DrugAndFrequencyGroup )
            {
                var item = nidaAssessFurtherInstance.ItemInstances.FirstOrDefault ( i => i.ItemDefinitionCode == itemDefinition.CodedConcept.Code );
                if (item == null)
                {
                    continue;
                }

                if (item.Value is Lookup )
                {
                    if ( otherDrug != string.Empty )
                    {
                        reportItems.Add ( new ReportItem (
                                              PropertyUtil.ExtractPropertyName<NidaReportData, List<ReportString>> ( r => r.SummaryItems ),
                                              null,
                                              new object[]
                                                  {
                                                      otherDrug,
                                                      nidaAssessFurtherResources.GetString ( "_" + ( (Lookup) item.Value ).CodedConcept.Code)
                                                  }
                                              )
                            {
                                ItemMetadata = new ItemMetadata {MetadataItems = new List<IMetadataItem> {new ReadonlyMetadataItem {IsReadonly = true}}}
                            } );
                        otherDrug = string.Empty;
                    }
                    else
                    {
                        var lookupName =
                            nidaAssessFurtherResources.GetString ( "_" + ( (Lookup) item.Value ).CodedConcept.Code );
                        if ( ( (Lookup) item.Value ).Value == 0 )
                        {
                            continue;
                        }
                        reportItems.Add ( new ReportItem (
                                              PropertyUtil.ExtractPropertyName<NidaReportData, List<ReportString>> ( r => r.SummaryItems ),
                                              null,
                                              new object[]
                                                  {
                                                      nidaAssessFurtherResources.GetString ( "_" + item.ItemDefinitionCode ),
                                                      lookupName
                                                  }
                                              )
                            {
                                ItemMetadata = new ItemMetadata {MetadataItems = new List<IMetadataItem> {new ReadonlyMetadataItem {IsReadonly = true}}}
                            } );
                    }
                }
                else
                {
                    otherDrug = item.Value.ToString ();
                }
            }
            reportModel.AddReportItem ( new ReportItem (
                                            PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.SummaryHeader ),
                                            null,
                                            new [] { dastInstance.Score.Value } ,
                                            reportItems.ToArray ()
                                            )
                {
                    ItemMetadata = new ItemMetadata {MetadataItems = new List<IMetadataItem> {new ReadonlyMetadataItem {IsReadonly = true}}}
                } );
            return reportModel;
        }

        #endregion
    }
}