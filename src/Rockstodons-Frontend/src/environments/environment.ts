
export const environment = {
  production: false,
  defaultLanguage: 'en-US',
  supportedLanguages: ['en-US'],

  // Source code for API Project to run on localhost
  apiEndpoint: 'https://localhost:7246/api/v1',
  apiUrl: 'https://localhost:7246/api/v1',
  // settings for connection to Duende IdentityServer
  oidc: {
    issuer: 'https://localhost:44310', // running on localhost
    clientId: 'RockstodonsClient', // client id setup in Duende IdentityServer
    responseType: 'code', //code flow PKCE
    redirectUri: window.location.origin,
    postLogoutRedirectUri: window.location.origin,
    silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',
    scope: 'openid profile email roles', // Ask offline_access to support refresh token refreshes
    useSilentRefresh: true, // Needed for Code Flow to suggest using iframe-based refreshes
    silentRefreshTimeout: 5000, // For faster testing
    timeoutFactor: 0.25, // For faster testing
    sessionChecksEnabled: false,
    showDebugInformation: false, // Also requires enabling "Verbose" level in devtools
    clearHashAfterLogin: false, // https://github.com/manfredsteyer/angular-oauth2-oidc/issues/457#issuecomment-431807040,
    nonceStateSeparator: 'semicolon', // Real semicolon gets mangled by IdentityServer's URI encoding
  }
};
