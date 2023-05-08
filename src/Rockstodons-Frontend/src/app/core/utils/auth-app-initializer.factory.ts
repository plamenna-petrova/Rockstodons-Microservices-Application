import { OAuth2Service } from '../services/oauth2.service';

export function authAppInitializerFactory(
  oauth2Service: OAuth2Service
): () => Promise<void> {
  return () => oauth2Service.runInitialLoginSequence();
}
