export function GetDatabaseName() {
  const uuid = crypto.randomUUID();
  return Promise.resolve(`Some Database: ${uuid}`);
}
