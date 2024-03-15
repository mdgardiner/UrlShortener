const findFirstExistingProperty = (properties, errorMessage) => {
  const notEmptyProperties = properties.filter(
    (property) => property !== undefined
  );
  if (notEmptyProperties.length === 0) {
    throw new Error(errorMessage);
  }
  return notEmptyProperties[0];
};

const resolveApiBaseUrl = () => {
  return findFirstExistingProperty(
    [
      window.env.REACT_APP_SHORTENER_API_BASE_URL,
      process.env.REACT_APP_SHORTENER_API_BASE_URL,
    ],
    "Failed to resolve api base url..."
  );
};

export const SHORTENER_API_BASE_URL = resolveApiBaseUrl();
