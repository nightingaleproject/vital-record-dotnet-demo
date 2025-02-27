module.exports = {
    rules: {
        "scope-enum": [0, "always", ["vrdr","bfdr","vr"]],
    }
}

const Configuration = {
    /*
     * Resolve and load @commitlint/config-conventional from node_modules.
     * Referenced packages must be installed
     */
    extends: ['@commitlint/config-conventional'],
    /*
     * Any rules defined here will override rules from @commitlint/config-conventional
     */
    rules: {
      'scope-enum': [2, 'always', ['vrdr','bfdr','vr','canary']],
    },
    /*
     * Array of functions that return true if commitlint should ignore the given message.
     * Given array is merged with predefined functions, which consist of matchers like:
     *
     * - 'Merge pull request', 'Merge X into Y' or 'Merge branch X'
     * - 'Revert X'
     * - 'v1.2.3' (ie semver matcher)
     * - 'Automatic merge X' or 'Auto-merged X into Y'
     *
     * To see full list, check https://github.com/conventional-changelog/commitlint/blob/master/%40commitlint/is-ignored/src/defaults.ts.
     * To disable those ignores and run rules always, set `defaultIgnores: false` as shown below.
     */
    ignores: [(commit) => commit === ''],
    /*
     * Whether commitlint uses the default ignore rules, see the description above.
     */
    defaultIgnores: true,
    /*
     * Custom URL to show upon failure
     */
    helpUrl:
      'https://github.com/conventional-changelog/commitlint/#what-is-commitlint',
  };
  
  module.exports = Configuration;