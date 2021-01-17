export interface Version {
  version: string;
  gitCommitHash?: string;
  versionLong?: string;
  gitTag?: string;
  versionDate: Date;
}
