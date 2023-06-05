import { Component } from '@angular/core';

@Component({
  selector: 'app-terms-and-conditions',
  templateUrl: './terms-and-conditions.component.html',
  styleUrls: ['./terms-and-conditions.component.scss']
})
export class TermsAndConditionsComponent {
  termsAndConditionsPanels = [
    {
      active: true,
      name: '1. General',
      content: 'Rockstodons is responsible for data processing on this website within the meaning ' +
        'of the European General Data Protection Regulation(GDPR). ' +
        'We respect your personal rights.We understand the importance of the personal information ' +
        'we receive from you as a user of our website.We respect the protection of your personal data ' +
        'and will only collect, store or process all data obtained in accordance with the relevant data ' +
        'protection regulations within the scope of our business purpose.',
      disabled: false
    },
    {
      active: false,
      disabled: false,
      name: '2. Definitions',
      content: 'Personal data is any information relating to an identified or' +
        'identifiable natural person. An identifiable natural person is one who can be identified, ' +
        'directly or indirectly, in particular by reference to an identifier such as a name, ' +
        'an identification number, location data, an online identifier or to one or more special ' +
        'features that express the physical , physiological, genetic, psychological, economic, cultural ' +
        'or social identity of that natural person.',
    },
    {
      active: false,
      name: '3. Legal basis of processing',
      disabled: false,
      content: 'If we obtain your consent for the processing of personal data, ' +
        'Article 6 Paragraph 1 Clause 1 Letter a of the European General Data Protection Regulation ' +
        '(GDPR) serves as the legal basis. \n' +
        'The processing of personal data required to fulfill a contract with you is based ' +
        'on Article 6 Paragraph 1 Sentence 1 lit. b GDPR. This also applies to processing operations ' +
        'that are necessary to carry out pre-contractual measures. ' +
        'Insofar as processing of personal data is necessary to fulfill a legal obligation to ' +
        'which our company is subject, this is done within the framework of Article 6 (1) sentence ' +
        '1 lit.c GDPR. If the processing is necessary to protect a legitimate interest of us ' +
        'or a third party and your interests, fundamental rights and fundamental freedoms ' +
        'do not outweigh the legitimate interest, Article 6 Paragraph 1 Sentence 1 lit.f GDPR serves ' +
        'as the legal basis for the processing.If information is stored on your end device, ' +
        'e.g.by means of cookies(see also Sections 5.3, 5.5 and 5.6 in particular), the admissibility ' +
        'of data use is also based on Section 25(1) TTDSG(consent) or in the case of mandatory storage ' +
        'processes according to § 25 paragraph 2 No. 1(communication process) or No. 2 ' +
        '(provision of a telemedia service) TTDSG.The legitimate interest of our company usually lies ' +
        'in the provision of the services we owe and / or the ongoing optimization of our ' +
        'services and presentations.'
    },
    {
      active: false,
      name: '4. Data Erasure and Retention Period',
      disabled: false,
      content: 'Your personal data will be deleted or blocked as soon as the purpose of storage ' +
        'no longer applies.Storage can also take place if this has been provided for by the European ' +
        'or national legislator in EU regulations, laws or other regulations. ' +
        'The data will also be blocked or deleted if a storage period prescribed by the standards ' +
        'mentioned expires, unless there is a need for further storage of the data for the conclusion ' +
        'or fulfillment of a contract.'
    },
    {
      active: false,
      name: '5. Collection of personal data',
      disabled: false,
      content: 'In principle, we do not collect or use any personal data when you visit our website. ' +
        'This only happens to the extent necessary to provide a functional website ' +
        'and our content and services.The collection and use of personal data of our users ' +
        'takes place regularly only with their consent.The situation is different if the ' +
        'processing of the data is permitted by statutory provisions.'
    }
  ];

  constructor() {

  }

  ngOnInit(): void {

  }
}
