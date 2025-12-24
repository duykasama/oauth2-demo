import { createFileRoute, redirect } from '@tanstack/react-router'

export const Route = createFileRoute('/google-oauth' as any)({
    loader: async ({ location }) => {
        const { code } = location.search as any;

        if (code) {
            var userInfo = await getUserInfo(code)
            localStorage.setItem('userInfo', userInfo)
        }

        throw redirect({
            to: '/' as any,
        })
    }
})

async function getUserInfo(code: string): Promise<string> {
    const url = '/api/auth/oauth/google'
    const request = { code }

    const res = await fetch(url, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request),
    })

    const json = await res.json()

    return JSON.stringify(json)
}
