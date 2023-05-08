
export const environment = {
  production: false,
  defaultLanguage: 'en-US',
  apiUrl: 'https://localhost:7246/api/v1',
  supportedLanguages: ['en-US'],
  apiEndpoint: 'https://localhost:7246/api/v1',
  oidc: {
    issuer: 'https://localhost:44310',
    clientId: 'RockstodonsClient',
    responseType: 'code',
    redirectUri: window.location.origin,
    postLogoutRedirectUri: window.location.origin,
    silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',
    scope: 'openid profile email roles app.api.employeeprofile.read',
    useSilentRefresh: true,
    silentRefreshTimeout: 50000,
    timeoutFactor: 0.25,
    sessionChecksEnabled: false,
    showDebugInformation: false,
    clearHashAfterLogin: false,
    nonceStateSeparator: 'semicolon'
  }
};
