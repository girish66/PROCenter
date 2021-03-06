﻿#region License Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Models;
    using Service.Message.Assessment;
    using Service.Message.Common.Lookups;
    using Service.Message.Message;
    using Service.Message.Patient;

    #endregion

    public class AssessmentController : BaseController
    {
        #region Fields

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        public AssessmentController ( IRequestDispatcherFactory requestDispatcherFactory, IResourcesManager resourcesManager )
            : base ( requestDispatcherFactory )
        {
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        public async Task<ActionResult> Create(Guid patientKey, Guid assessmentDefinitionKey, Guid? workflowKey = null)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new CreateAssessmentRequest
                {
                    AssessmentDefinitionKey = assessmentDefinitionKey,
                    PatientKey = patientKey,
                    WorkflowKey = workflowKey
                } );
            var response = await requestDispatcher.GetAsync<CreateAssessmentResponse> ();
            return RedirectToAction ( "Edit", new {key = response.AssessmentInstanceKey, patientKey} );
        }

        public async Task<ActionResult> CreateForSelfAdministration(Guid patientKey, Guid assessmentDefinitionKey, bool administerNow = false, Guid? workflowKey = null)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new CreateAssessmentRequest
            {
                AssessmentDefinitionKey = assessmentDefinitionKey,
                PatientKey = patientKey,
                WorkflowKey = workflowKey,
                ForSelfAdministration = true
            });
            var response = await requestDispatcher.GetAsync<CreateAssessmentResponse>();
            if ( administerNow )
            {
                return RedirectToAction ( "Edit", new {key = response.AssessmentInstanceKey, patientKey} );
            }
            return RedirectToAction ( "Index", "Patient", new {key = patientKey} );
        }

        public virtual async Task<ActionResult> Edit ( Guid key, Guid patientKey )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetSectionDtoByKeyRequest {Key = key} );
            
            requestDispatcher.Add ( new GetPatientDtoByKeyRequest {PatientKey = patientKey} );

            var response = await requestDispatcher.GetAsync<GetSectionDtoByKeyResponse> ();
            var patientResponse = requestDispatcher.Get<GetPatientDtoResponse> ();

            ViewData["Patient"] = patientResponse.DataTransferObject;
            var messages = new List<IMessageDto>((TempData["Messages"] as IEnumerable<IMessageDto>) ?? Enumerable.Empty<IMessageDto>());
            messages.AddRange( response.Messages.Where( m => !messages.Contains(m)));
            ViewData["Messages"] = messages;
            var sectionDto = response.DataTransferObject;

            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName ( sectionDto.AssessmentName );
            ViewData["ResourcesManager"] = _resourcesManager;
            
            return View ( sectionDto );
        }

        [HttpPost]
        public virtual async Task<JsonResult> Edit ( Guid key, Guid patientKey, string itemDefinitionCode, string value, bool isLookup = false )
        {
            ItemDto item;
            if (!isLookup)
            {
                item = new ItemDto
                    {
                        ItemDefinitionCode = itemDefinitionCode,
                        Value = value
                    };
            }
            else 
            {
                item = new ItemDto
                    {
                        ItemDefinitionCode = itemDefinitionCode,
                        Value = new LookupDto { Code = value }
                    };
            }

            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new SaveAssessmentItemRequest
                {
                    Key = key,
                    Item = item,
                } );

            var response = await requestDispatcher.GetAsync<SaveAssessmentItemResponse> ();
            //TODO: Error handling

            return new JsonResult
                {
                    Data = new { response.CanSubmit }
                };
        }

        [HttpPost]
        public async Task<PartialViewResult> Submit(Guid key, Guid patientKey)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new SubmitAssessmentRequest { AssessmentKey = key, Submit = true });
            requestDispatcher.Add(new GetPatientDtoByKeyRequest { PatientKey = patientKey });
            var response = await requestDispatcher.GetAsync<SubmitAssessmentResponse>();
            var patientResponse = requestDispatcher.Get<GetPatientDtoResponse>();

            ViewData["Patient"] = patientResponse.DataTransferObject;
            ViewData["ResourcesManager"] = _resourcesManager;

            return PartialView("ScoreHeader", new ScoreHeaderViewModel { Score = response.ScoreDto, Messages = response.Messages });
        }

        #endregion
    }
}