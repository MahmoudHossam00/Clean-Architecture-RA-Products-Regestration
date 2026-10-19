export function safeReturnUrl(
  value: string | null,
  fallback = '/'
): string {

  if (
    !value ||
    !value.startsWith('/') ||
    value.startsWith('//')
  ) {

    return fallback;
  }


  if (
    value.startsWith(
      '/login'
    ) ||
    value.startsWith(
      '/first-login'
    )
  ) {

    return fallback;
  }


  return value;
}